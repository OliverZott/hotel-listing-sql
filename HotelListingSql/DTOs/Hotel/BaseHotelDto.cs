using System.ComponentModel.DataAnnotations;

namespace HotelListingSql.DTOs.Hotel;

public abstract class BaseHotelDto
{
    [Microsoft.Build.Framework.Required]
    public string? Name { get; set; }
    public string? Address { get; set; }
    public double? Rating { get; set; }

    [Microsoft.Build.Framework.Required]
    [Range(1, int.MaxValue)]
    public int CountryId { get; set; }
}
