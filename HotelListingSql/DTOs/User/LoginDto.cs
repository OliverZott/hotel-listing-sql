using System.ComponentModel.DataAnnotations;

namespace HotelListingSql.DTOs.User;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [StringLength(15, ErrorMessage = "Password is limited to {2}-{1} characters", MinimumLength = 6)]
    public string Password { get; set; }
}