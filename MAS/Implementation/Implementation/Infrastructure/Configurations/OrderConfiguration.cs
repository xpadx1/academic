using Implementation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Implementation.Infrastructure.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(o => o.PaymentMethod)
            .HasMaxLength(50);

        builder.Property(o => o.PaymentReference)
            .HasMaxLength(100);

        builder.Property(o => o.IsPaid)
            .IsRequired();

        builder.HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(o => o.Waiter)
            .WithMany()
            .HasForeignKey(o => o.WaiterId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}