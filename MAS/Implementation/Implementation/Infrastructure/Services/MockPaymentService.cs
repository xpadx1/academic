using Implementation.Application.Interfaces;

namespace Implementation.Infrastructure.Services;

public class MockPaymentService : IPaymentService
{
    public Task<PaymentResult> ProcessPaymentAsync(
        PaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Amount <= 0)
        {
            return Task.FromResult(new PaymentResult(
                false,
                ErrorMessage: "Payment amount must be greater than zero."));
        }

        var reference = $"PAY-{Guid.NewGuid():N}".ToUpperInvariant();
        return Task.FromResult(new PaymentResult(true, reference));
    }
}
