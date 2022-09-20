using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelListingSql.Data;

// Contract between application and database
public class HotelListingDbContext : IdentityDbContext<ApiUser>
{
    public HotelListingDbContext(DbContextOptions contextOptions) : base(contextOptions)
    {

    }

    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Country>? Countries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Country>().HasData(
            new Country { Id = 1, Name = "Austria", ShortName = "AT" },
            new Country { Id = 2, Name = "Germany", ShortName = "DE" }
        );
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Hotel>().HasData(
            new Hotel { Id = 1, Name = "Motel One", Address = "Address Motel One", CountryId = 1, Rating = 3 },
            new Hotel { Id = 2, Name = "Hotel Post", Address = "Leermos", CountryId = 1, Rating = 4 }
        );
    }
}