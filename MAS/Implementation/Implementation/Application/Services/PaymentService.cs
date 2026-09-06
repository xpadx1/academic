using Implementation.Application.Interfaces;

namespace Implementation.Application.Services;

public class PaymentService : IPaymentService
{
    public Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        var reference = $"PAY-{Guid.NewGuid():N}".ToUpperInvariant();
        return Task.FromResult(new PaymentResult(true, reference, null));
    }
}