using Implementation.Domain.Enums;

namespace Implementation.Domain.Entities;

public class Waiter : Employee
{
    public Waiter(
        string firstName,
        string lastName,
        string email,
        GenderType gender,
        RankType rank,
        DateTime employmentDate,
        decimal minimalSalary,
        string? phoneNumber = null)
        : base(firstName, lastName, email, gender, rank, employmentDate, minimalSalary, phoneNumber)
    {
    }

    protected Waiter()
    {
    }
}