using HotelListingSql.Data;

namespace HotelListingSql.Core.Contracts;

public interface IHotelRepository : IGenericRepository<Hotel>
{
    Task<Hotel?> GetDetails(int id);
}
