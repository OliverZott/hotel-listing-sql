using HotelListingSql.Data;

namespace HotelListingSql.Contracts;

public interface ICountriesRepository : IGenericRepository<Country>
{
    Task<Country?> GetDetails(int id);
}