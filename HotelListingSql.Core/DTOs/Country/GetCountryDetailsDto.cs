using HotelListingSql.Core.DTOs.Hotel;

namespace HotelListingSql.Core.DTOs.Country;

public class GetCountryDetailsDto : BaseCountryDto
{
    public int Id { get; set; }

    public List<GetHotelDto> Hotels { get; set; }
}
