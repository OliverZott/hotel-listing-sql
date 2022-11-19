using AutoMapper;
using HotelListingSql.Contracts;
using HotelListingSql.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListingSql.Repositories;

public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
{
    private readonly HotelListingDbContext _context;
    private readonly IMapper _mapper;

    public HotelRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
        _mapper = mapper;
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
