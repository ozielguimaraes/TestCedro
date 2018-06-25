using TestCedro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace TestCedro.Infra.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(c => c.UserId);

            builder.Property(c => c.UserId)
                   .ValueGeneratedOnAdd();  

            builder.Property(c => c.FirstName)
                   .HasMaxLength(30)
                   .IsRequired();

            builder.Property(c => c.Email)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(c => c.LastName)
                   .HasMaxLength(30)
                   .IsRequired();

            builder.Property(c => c.IsApproved)
                   .IsRequired();

            builder.Property(c => c.IsLockedOut)
                   .IsRequired();
        }
    }
}