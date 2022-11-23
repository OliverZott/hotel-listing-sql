using HotelListingSql.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

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
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new CountryConfiguration());
        modelBuilder.ApplyConfiguration(new HotelConfiguration());
    }

    // Factory because something with scaffolding controllers is broken after refactoring
    // Basically similar to actions in Program.cs
    public class HotelListingDbContextFactory : IDesignTimeDbContextFactory<HotelListingDbContext>
    {
        public HotelListingDbContext CreateDbContext(string[] args)
        {
            // fetch configuration
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // get connection string
            var optionsBuilder = new DbContextOptionsBuilder<HotelListingDbContext>();
            var connectionString = config.GetConnectionString("HotelListingDbConnectionString");
            optionsBuilder.UseNpgsql(connectionString);
            return new HotelListingDbContext(optionsBuilder.Options);
        }
    }
}