using HotelListingSql.Core.DTOs.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListingSql.Core.Contracts;

public interface IAuthenticationRepository
{
    Task<IEnumerable<IdentityError>> Register(ApiUserDto apiUserDto);
    Task<AuthResponseDto?> Login(LoginDto loginDto);
    Task<string> CreateRefreshToken();
    Task<AuthResponseDto?> VerifyRefreshToken(AuthResponseDto request);
}
