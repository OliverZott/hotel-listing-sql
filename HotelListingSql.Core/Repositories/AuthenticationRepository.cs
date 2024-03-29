﻿using AutoMapper;
using HotelListingSql.Core.Contracts;
using HotelListingSql.Core.DTOs.User;
using HotelListingSql.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace HotelListingSql.Core.Repositories;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly IMapper _mapper;
    private readonly UserManager<ApiUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationRepository> _logger;
    private ApiUser? _user;

    private const string _loginProvider = "HotelListingApi";
    private const string _refreshToken = "RefreshToken";


    public AuthenticationRepository(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration, ILogger<AuthenticationRepository> logger)
    {
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IEnumerable<IdentityError>> Register(ApiUserDto apiUserDto)
    {
        _user = _mapper.Map<ApiUser>(apiUserDto);
        _user.UserName = apiUserDto.Email;
        var result = await _userManager.CreateAsync(_user, apiUserDto.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(_user, "User");
        }

        return result.Errors;
    }

    public async Task<AuthResponseDto?> Login(LoginDto loginDto)
    {
        _logger.LogInformation($"Looking for user with email {loginDto.Email}");
        _user = await _userManager.FindByEmailAsync(loginDto.Email);
        var isValidUser = await _userManager.CheckPasswordAsync(_user, loginDto.Password);

        if (_user == null || isValidUser == false)
        {
            _logger.LogWarning($"User with email {loginDto.Email} not found");
            return null;
        }
        var token = await GenerateToken();
        return new AuthResponseDto
        {
            Token = token,
            UserId = _user.Id,
            RefreshToken = await CreateRefreshToken()
        };
    }

    // remove old token, generate new one, store it and returns it
    public async Task<string> CreateRefreshToken()
    {
        await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);
        var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);
        await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, newRefreshToken);
        return newRefreshToken;
    }

    public async Task<AuthResponseDto?> VerifyRefreshToken(AuthResponseDto request)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
        var userName = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)?.Value;
        _user = await _userManager.FindByNameAsync(userName);

        if (_user == null || _user.Id != request.UserId) return null;

        var isValidRefreshToken =
            await _userManager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken, request.RefreshToken);

        if (isValidRefreshToken)
        {
            var token = await GenerateToken();
            return new AuthResponseDto
            {
                Token = token,
                UserId = _user.Id,
                RefreshToken = await CreateRefreshToken()
            };
        }

        await _userManager.UpdateSecurityStampAsync(_user);
        return null;
    }

    public async Task<string> GenerateToken()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roles = await _userManager.GetRolesAsync(_user);
        var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

        var userClaims = await _userManager.GetClaimsAsync(_user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, _user.Email),
            new Claim("uid", _user.Id)
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
