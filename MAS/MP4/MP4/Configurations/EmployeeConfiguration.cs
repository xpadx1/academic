using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MP4.Models;

namespace MP4.Repositories;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(e => e.Rank)
            .HasConversion<string>();

        builder.Property(e => e.EmploymentDate)
            .HasConversion<DateTime>();

        builder.Property(e => e.Education)
            .HasConversion(
                v => string.Join(';', v),
                v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList());
    }
}