using Microsoft.EntityFrameworkCore;

namespace HotelListingSql.Data;

// Contract between application and database
public class HotelListingDbContext : DbContext
{
    public HotelListingDbContext(DbContextOptions contextOptions) : base(contextOptions) { }

    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Country>? Countries { get; set; }

}