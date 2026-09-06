using Implementation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Infrastructure.Persistence;

public class RestaurantDbContext : DbContext
{
    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
        : base(options)
    {
    }

public DbSet<Customer> Customers => Set<Customer>();

public DbSet<Employee> Employees => Set<Employee>();

public DbSet<Menu> Menus => Set<Menu>();

    public DbSet<MenuItem> MenuItems => Set<MenuItem>();

    public DbSet<Ingredient> Ingredients => Set<Ingredient>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RestaurantDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}