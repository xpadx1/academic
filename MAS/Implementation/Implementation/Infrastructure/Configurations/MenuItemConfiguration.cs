using Implementation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Implementation.Infrastructure.Configurations;

public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.ToTable("MenuItems");

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Description)
            .HasMaxLength(500);

        builder.Property(m => m.BasePrice)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(m => m.Calories)
            .IsRequired();

        builder.HasDiscriminator<string>("ItemType")
            .HasValue<StandardItem>("Standard")
            .HasValue<SeasonalItem>("Seasonal");

        builder.HasMany(m => m.Ingredients)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "MenuItemIngredient",
                j => j.HasOne<Ingredient>().WithMany().HasForeignKey("IngredientId"),
                j => j.HasOne<MenuItem>().WithMany().HasForeignKey("MenuItemId"),
                j =>
                {
                    j.HasKey("MenuItemId", "IngredientId");
                });
    }
}