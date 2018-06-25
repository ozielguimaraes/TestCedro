using TestCedro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace TestCedro.Infra.Data.Mappings
{
    public class DishMap : IEntityTypeConfiguration<Dish>
    {
        public void Configure(EntityTypeBuilder<Dish> builder)
        {
            builder.HasKey(c => c.DishId);

            builder.Property(c => c.DishId)
                   .ValueGeneratedOnAdd();  
        }
    }
}
