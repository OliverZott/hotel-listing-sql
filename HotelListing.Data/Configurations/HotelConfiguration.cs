using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListingSql.Data.Configurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.HasData(
            new Hotel { Id = 1, Name = "Motel One", Address = "Address Motel One", CountryId = 1, Rating = 3 },
            new Hotel { Id = 2, Name = "Hotel Post", Address = "Leermos", CountryId = 1, Rating = 4 }
        );
    }
}
