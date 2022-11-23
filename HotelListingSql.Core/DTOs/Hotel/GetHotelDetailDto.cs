using HotelListingSql.Core.DTOs.Country;

namespace HotelListingSql.Core.DTOs.Hotel;

public class GetHotelDetailDto : BaseHotelDto
{
    public int Id { get; set; }
    public GetCountryDto? Country { get; set; }
}
