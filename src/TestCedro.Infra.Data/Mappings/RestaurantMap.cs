using TestCedro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace TestCedro.Infra.Data.Mappings
{
    public class RestaurantMap : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.HasKey(c => c.RestaurantId);

            builder.Property(c => c.RestaurantId)
                   .ValueGeneratedOnAdd();  
        }
    }
}
