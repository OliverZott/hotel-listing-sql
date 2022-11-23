using AutoMapper;
using HotelListingSql.Core.DTOs.Country;
using HotelListingSql.Core.DTOs.Hotel;
using HotelListingSql.Core.DTOs.User;
using HotelListingSql.Data;

namespace HotelListingSql.Core.Configurations;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Country, CreateCountryDto>().ReverseMap();
        CreateMap<Country, GetCountryDto>().ReverseMap();
        CreateMap<Country, GetCountryDetailsDto>().ReverseMap();
        CreateMap<Country, UpdateCountryDto>().ReverseMap();

        CreateMap<Hotel, CreateHotelDto>().ReverseMap();
        CreateMap<Hotel, GetHotelDetailDto>().ReverseMap();
        CreateMap<Hotel, GetHotelDto>().ReverseMap();
        CreateMap<Hotel, UpdateHotelDto>().ReverseMap();

        CreateMap<ApiUser, ApiUserDto>().ReverseMap();
    }
}
