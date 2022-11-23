using System.ComponentModel.DataAnnotations;

namespace HotelListingSql.Core.DTOs.User;

public class ApiUserDto : LoginDto
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
}