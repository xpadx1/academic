using Implementation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Implementation.Infrastructure.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(i => i.Quantity)
            .IsRequired();

        builder.Property(i => i.UnitPrice)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.HasOne(i => i.MenuItem)
            .WithMany()
            .HasForeignKey(i => i.MenuItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}