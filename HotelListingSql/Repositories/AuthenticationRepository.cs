using AutoMapper;
using HotelListingSql.Contracts;
using HotelListingSql.Data;
using HotelListingSql.DTOs.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace HotelListingSql.Repositories;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly IMapper _mapper;
    private readonly UserManager<ApiUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthenticationRepository(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
    {
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<IEnumerable<IdentityError>> Register(ApiUserDto apiUserDto)
    {
        var user = _mapper.Map<ApiUser>(apiUserDto);
        user.UserName = apiUserDto.Email;
        var result = await _userManager.CreateAsync(user, apiUserDto.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
        }

        return result.Errors;
    }

    public async Task<AuthResponseDto?> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        var isValidUser = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (user == null || isValidUser == false)
        {
            return null;
        }
        var token = await GenerateToken(user);
        return new AuthResponseDto
        {
            Token = token,
            UserId = user.Id
        };
    }

    public async Task<string> GenerateToken(ApiUser apiUser)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roles = await _userManager.GetRolesAsync(apiUser);
        var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

        var userClaims = await _userManager.GetClaimsAsync(apiUser);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, apiUser.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, apiUser.Email),
            new Claim("uid", apiUser.Id)
        }.Union(userClaims).Union(roleClaims);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
            signingCredentials: credentials
        );


        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
