using Implementation.Domain.Enums;
using Implementation.Domain.Interfaces;

namespace Implementation.Domain.Entities;

public abstract class Employee : Person
{
    private const decimal MinimalSalaryThreshold = 3000m;

    private string _educationRecordsSerialized = string.Empty;

    protected Employee(
        string firstName,
        string lastName,
        string email,
        GenderType gender,
        RankType rank,
        DateTime employmentDate,
        decimal minimalSalary,
        string? phoneNumber = null)
        : base(firstName, lastName, email, gender, phoneNumber)
    {
        Rank = rank;
        EmploymentDate = employmentDate;
        MinimalSalary = minimalSalary;
    }

    protected Employee()
    {
    }

    public RankType Rank { get; private set; }

    public DateTime EmploymentDate { get; private set; }

    public decimal MinimalSalary { get; private set; }

    public IReadOnlyList<string> GetEducationRecords() =>
        _educationRecordsSerialized
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .ToArray();

    public void AddEducationRecord(string record)
    {
        if (string.IsNullOrWhiteSpace(record))
        {
            throw new ArgumentException("Education record cannot be empty.", nameof(record));
        }

        _educationRecordsSerialized =
            _educationRecordsSerialized.Length == 0
                ? record.Trim()
                : $"{_educationRecordsSerialized};{record.Trim()}";
    }

    public int GetTenureMonth(IDateTimeProvider dateTimeProvider)
    {
        var now = dateTimeProvider.UtcNow;
        var months = (now.Year - EmploymentDate.Year) * 12 + now.Month - EmploymentDate.Month;
        return months < 0 ? 0 : months;
    }

    public void SetMinimalSalary(decimal salary)
    {
        if (salary <= MinimalSalaryThreshold)
        {
            throw new ArgumentException(
                $"Minimal salary must be greater than {MinimalSalaryThreshold}.", nameof(salary));
        }

        MinimalSalary = salary;
    }
}