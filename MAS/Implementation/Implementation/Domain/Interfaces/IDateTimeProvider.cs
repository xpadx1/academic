namespace Implementation.Domain.Interfaces;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}