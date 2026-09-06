using Implementation.Application.DTOs;
using Implementation.Application.Exceptions;
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

public async Task<CustomerResponse> GetCustomerAsync(
        int customerId,
        CancellationToken cancellationToken = default)
    {
        var customer = await _dbContext.Customers
            .AsNoTracking()
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken)
            ?? throw new NotFoundException($"Customer with id {customerId} was not found.");

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