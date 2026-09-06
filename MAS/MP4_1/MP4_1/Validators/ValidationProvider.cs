using System.ComponentModel.DataAnnotations;

namespace MP4_1.Validators;

public class Validator
{
    public void ValidateModel(object model)
    {
        var context = new ValidationContext(model, serviceProvider: null, items: null);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(model, context, results, validateAllProperties: true))
        {
            foreach (var validationResult in results)
            {
                Console.WriteLine($"Validation Error: {validationResult.ErrorMessage}");
            }

            throw new ValidationException("Validation failed for the object.");
        }
    }

}