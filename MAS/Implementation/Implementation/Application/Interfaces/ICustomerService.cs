using Implementation.Application.DTOs;

namespace Implementation.Application.Interfaces;

public interface ICustomerService
{
    Task<CustomerResponse> GetCustomerAsync(int customerId);
}
