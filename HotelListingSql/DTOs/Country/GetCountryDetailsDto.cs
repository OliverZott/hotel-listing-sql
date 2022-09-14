using HotelListingSql.DTOs.Hotel;

namespace HotelListingSql.DTOs.Country;

public class GetCountryDetailsDto : BaseCountryDto
{
    public int Id { get; set; }

    public List<GetHotelDto> Hotels { get; set; }
}
