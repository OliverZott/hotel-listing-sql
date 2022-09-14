using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListingSql.DTOs.Hotel;

public class GetHotelDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public double? Rating { get; set; }

    [ForeignKey(nameof(CountryId))]
    public int CountryId { get; set; }
}
