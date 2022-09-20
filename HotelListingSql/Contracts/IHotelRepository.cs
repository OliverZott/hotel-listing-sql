using HotelListingSql.Data;

namespace HotelListingSql.Contracts;

public interface IHotelRepository : IGenericRepository<Hotel>
{
    Task<Hotel?> GetDetails(int id);
}
