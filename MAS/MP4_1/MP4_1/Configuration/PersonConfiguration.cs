using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MP4.Models;

namespace MP4.Configurations;

public class PersonConfiguration
    : IEntityTypeConfiguration<Person>
{
    public void Configure(
        EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Email)
            .IsRequired();

        builder.HasDiscriminator<string>("PersonType")
            .HasValue<Customer>("Customer")
            .HasValue<Waiter>("Waiter")
            .HasValue<Manager>("Manager");

        builder.UseTphMappingStrategy();
    }
}