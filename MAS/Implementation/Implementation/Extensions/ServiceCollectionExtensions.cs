using Implementation.Application.Interfaces;
using Implementation.Application.Services;
using Implementation.Domain.Interfaces;
using Implementation.Infrastructure.Persistence;
using Implementation.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ICustomerService, CustomerService>();

        return services;
    }

public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RestaurantDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddSingleton<IPaymentService, MockPaymentService>();

        return services;
    }
}