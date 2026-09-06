using Implementation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Implementation.Infrastructure.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("People");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.PhoneNumber)
            .HasMaxLength(30);

        builder.Property(p => p.Gender)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasDiscriminator<string>("PersonType")
            .HasValue<Customer>("Customer")
            .HasValue<Cook>("Cook")
            .HasValue<Waiter>("Waiter")
            .HasValue<Manager>("Manager");
    }
}