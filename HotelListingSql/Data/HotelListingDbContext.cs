using Microsoft.EntityFrameworkCore;

namespace HotelListingSql.Data;

// Contract between app and database
public class HotelListingDbContext : DbContext
{
    public HotelListingDbContext(DbContextOptions contextOptions) : base(contextOptions)
    {

    }
}
