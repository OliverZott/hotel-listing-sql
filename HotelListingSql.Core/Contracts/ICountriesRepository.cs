using HotelListingSql.Data;

namespace HotelListingSql.Core.Contracts;

public interface ICountriesRepository : IGenericRepository<Country>
{
    Task<Country?> GetDetails(int id);
}