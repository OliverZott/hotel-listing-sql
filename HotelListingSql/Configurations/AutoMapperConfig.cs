using AutoMapper;
using HotelListingSql.Data;
using HotelListingSql.DTOs.Country;
using HotelListingSql.DTOs.Hotel;
using HotelListingSql.DTOs.User;

namespace HotelListingSql.Configurations;

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
