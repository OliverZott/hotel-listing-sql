using HotelListingSql.Contracts;
using HotelListingSql.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListingSql.Repositories;

public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
{
    private readonly HotelListingDbContext _context;

    public HotelRepository(HotelListingDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Hotel?> GetDetails(int id)
    {
        if (_context.Hotels is null)
        {
            return null;
        }

        return await _context.Hotels.Include(q => q.Country).FirstOrDefaultAsync(q => q.Id == id);
    }
}
