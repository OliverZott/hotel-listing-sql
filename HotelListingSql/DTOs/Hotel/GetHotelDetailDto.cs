using HotelListingSql.DTOs.Country;

namespace HotelListingSql.DTOs.Hotel;

public class GetHotelDetailDto : BaseHotelDto
{
    public int Id { get; set; }
    public GetCountryDto? Country { get; set; }
}
