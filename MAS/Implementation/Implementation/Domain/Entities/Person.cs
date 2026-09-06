using Implementation.Domain.Enums;

namespace Implementation.Domain.Entities;

public abstract class Person
{
    protected Person(string firstName, string lastName, string email, GenderType gender, string? phoneNumber = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Gender = gender;
        PhoneNumber = phoneNumber;
    }

    protected Person()
    {
    }

    public int Id { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string Email { get; private set; }

    public string? PhoneNumber { get; private set; }

    public GenderType Gender { get; private set; }
}