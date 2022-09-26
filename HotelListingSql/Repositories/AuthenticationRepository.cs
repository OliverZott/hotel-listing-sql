using AutoMapper;
using HotelListingSql.Contracts;
using HotelListingSql.Data;
using HotelListingSql.DTOs.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListingSql.Repositories;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly IMapper _mapper;
    private readonly UserManager<ApiUser> _userManager;

    public AuthenticationRepository(IMapper mapper, UserManager<ApiUser> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
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

    public async Task<bool> Login(LoginDto loginDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            return await _userManager.CheckPasswordAsync(user, loginDto.Password);
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
