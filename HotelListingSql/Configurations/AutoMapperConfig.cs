using AutoMapper;
using HotelListingSql.Data;
using HotelListingSql.DTOs.Country;
using HotelListingSql.DTOs.Hotel;

namespace HotelListingSql.Configurations;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Country, CreateCountryDto>().ReverseMap();
        CreateMap<Country, GetCountryDto>().ReverseMap();
        CreateMap<Country, GetCountryDetailsDto>().ReverseMap();
        CreateMap<Country, UpdateCountryDto>().ReverseMap();
        CreateMap<Hotel, GetHotelDto>().ReverseMap();
    }
}
