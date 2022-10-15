using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShopAPI.Entities.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> eb)
        {
            //eb.HasOne(x => x.User)
            //    .WithOne(u => u.Address)
            //    .HasForeignKey<User>(a => a.AddressId);
            eb.Property(x => x.City).IsRequired().HasMaxLength(50);
            eb.Property(x => x.Street).IsRequired().HasMaxLength(30);
            eb.Property(x => x.ZipCode).IsRequired().HasMaxLength(10);
        }

    }
}
