using HotelListingSql.Contracts;
using HotelListingSql.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListingSql.Repositories;

public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
{
    private readonly HotelListingDbContext _context;

    public CountriesRepository(HotelListingDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Country?> GetDetails(int id)
    {
        if (_context.Countries is null)
        {
            return null;
        }
        return await _context.Countries.Include(q => q.Hotels).FirstOrDefaultAsync(q => q.Id == id);
    }
}
