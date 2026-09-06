using Implementation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Implementation.Infrastructure.Configurations;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Description)
            .HasMaxLength(500);

        builder.Property(m => m.IsAvailable)
            .IsRequired();

        builder.HasMany(m => m.Items)
            .WithOne(i => i.Menu)
            .HasForeignKey(i => i.MenuId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}