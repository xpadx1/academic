using Implementation.Application.DTOs;
using Implementation.Application.Interfaces;
using Implementation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly RestaurantDbContext _dbContext;

    public CustomerService(RestaurantDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CustomerResponse> GetCustomerAsync(int customerId)
    {
        var customer = await _dbContext.Customers
            .AsNoTracking()
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Id == customerId);

        return new CustomerResponse(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.PhoneNumber,
            customer.Gender.ToString(),
            customer.MemberSinceDate,
            customer.GetOrdersAmount());
    }
}