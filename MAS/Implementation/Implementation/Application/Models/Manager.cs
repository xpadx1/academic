using Implementation.Domain.Enums;

namespace Implementation.Domain.Entities;

public class Manager : Employee
{
    public Manager(
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

    protected Manager()
    {
    }

    public void CancelOrder(Order order)
    {
        order.Cancel();
    }
}