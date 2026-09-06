using System.Text.Json.Serialization;

namespace Implementation.Application.Interfaces;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentMethodType
{
    Card,
    Cash
}

public record PaymentRequest(
    PaymentMethodType Method,
    decimal Amount);

public record PaymentResult(
    bool Success,
    string? Reference = null,
    string? ErrorMessage = null);

public interface IPaymentService
{
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
}