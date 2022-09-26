using HotelListingSql.DTOs.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListingSql.Contracts;

public interface IAuthenticationRepository
{
    Task<IEnumerable<IdentityError>> Register(ApiUserDto apiUserDto);
    Task<bool> Login(LoginDto loginDto);
}
