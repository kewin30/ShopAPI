using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShopAPI.Entities.Configurations
{
    public class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure(EntityTypeBuilder<OrderStatus> builder)
        {
            builder.HasData(
                new OrderStatus() { Id = 1, Value = "Wysłane" },
                new OrderStatus() { Id = 2, Value = "Opłacone" },
                new OrderStatus() { Id = 3, Value = "W Realizacji"},
                new OrderStatus() { Id = 4, Value = "Do Zrobienia" }
                );
            builder.Property(x => x.Value)
                .IsRequired()
                .HasMaxLength(30);
        }
    }
}
