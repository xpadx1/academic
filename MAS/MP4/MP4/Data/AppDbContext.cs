using Microsoft.EntityFrameworkCore;
using MP4.Models;

namespace MP4.Data;

public class AddDbContext : DbContext
{
    public DbSet<Person> Persons => Set<Person>();
    /*public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Employee> Employees => Set<Employee>();*/

    public AddDbContext(DbContextOptions<AddDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AddDbContext).Assembly);
    }
}