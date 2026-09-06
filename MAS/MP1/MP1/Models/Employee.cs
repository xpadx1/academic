using MP1.Enums;

namespace MP1.Models.People;

public class Employee : Person
{
    private RankType Rank;
    private int CurrentSalary;
    private DateTime EmploymentDate;
    private string[] Education;
    private int TenureMonth;

    public Employee(string firstName, string lastName, string email, string? phoneNumber, GenderType gender, RankType rank,
        int currentSalary, DateTime employmentDate, string[] education, int tenureMonth) : base(firstName, lastName, email, gender, phoneNumber)
    {
        
    }
}