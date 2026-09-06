using MP1.Enums;
using MP1.Services;

namespace MP1.Models.People;

public abstract class Person
{
    private HelperMethods helper = new HelperMethods();
    
    private string _firstName;
    private string _lastName;
    private string _email;
    private string? _phoneNumber;
    private GenderType Gender { get; set; }

    public Person(string firstName, string lastName, string email, GenderType gender, string? phoneNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Gender = gender;
        PhoneNumber = phoneNumber;
    }

    public string FirstName
    {
        get => _firstName;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 2)
            {
                throw new ArgumentException("Name field should be longer than 2 characters");
            }
            if (!value.All(char.IsLetter))
            {
                throw new ArgumentException("Name cannot contain numbers or special symbols");
            }
            _firstName = value;
        }
    }
    
    public string LastName
    {
        get => _lastName;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 2)
            {
                throw new ArgumentException("Last Name field should be longer than 2 characters");
            }
            if (!value.All(char.IsLetter))
            {
                throw new ArgumentException("Last name cannot contain numbers or special symbols");
            }
            _lastName = value;
        }
    }
    
    public string Email
    {
        get => _email;
        set
        {
            if (!helper.isValidEmail(value))
            {
                throw new ArgumentException("Invalid email address");
            }
            _email = value;
        }
    }
    
    public string? PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if (value != null)
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 6 || value.Length > 14)
                {
                    throw new ArgumentException("Phone Number field should be longer than 6 characters or shorter than 14");
                }
                if (!value.All(char.IsDigit))
                {
                    throw new ArgumentException("Phone number should only contain numbers");
                }
            }

            _phoneNumber = value;

        }
    }
}