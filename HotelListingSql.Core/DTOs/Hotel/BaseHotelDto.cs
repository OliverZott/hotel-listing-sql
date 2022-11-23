using System.ComponentModel.DataAnnotations;

namespace HotelListingSql.Core.DTOs.Hotel;

public abstract class BaseHotelDto
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public double? Rating { get; set; }

    [Range(1, int.MaxValue)]
    public int CountryId { get; set; }
}
