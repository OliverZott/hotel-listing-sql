using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListingSql.Data.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasData(
            new Country { Id = 1, Name = "Austria", ShortName = "AT" },
            new Country { Id = 2, Name = "Germany", ShortName = "DE" }
        );
    }
}
