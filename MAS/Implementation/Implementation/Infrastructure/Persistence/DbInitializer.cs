using Implementation.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Infrastructure.Persistence;

public static class DbInitializer
{
    public static void Initialize(RestaurantDbContext context)
    {
        context.Database.Migrate();
        SeedData.Seed(context);
    }
}