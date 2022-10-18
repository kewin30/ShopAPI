using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShopAPI.Entities.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            //builder
            //    .HasOne<Product>(x => x.Products)
            //    .WithMany(p => p.Orders)
            //    .HasForeignKey(x => x.ProductId);

            //builder.HasOne(x => x.Products)
            //    .WithOne(x => x.Orders)
            //    .HasForeignKey<Product>(x => x.OrderId);
            builder.Property(x => x.StatusId).HasDefaultValue(4);
        }
    }
}
