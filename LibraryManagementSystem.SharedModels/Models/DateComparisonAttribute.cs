using System.ComponentModel.DataAnnotations;

public class DateComparisonAttribute : ValidationAttribute
{
    private readonly string _startDatePropertyName;

    public DateComparisonAttribute(string startDatePropertyName)
    {
        _startDatePropertyName = startDatePropertyName;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var startDate = (DateTime)validationContext.ObjectType.GetProperty(_startDatePropertyName).GetValue(validationContext.ObjectInstance, null);
        var endDate = (DateTime)value;

        if (startDate > endDate)
        {
            return new ValidationResult(ErrorMessage ?? "End Date must be greater than Start Date.");
        }

        return ValidationResult.Success;
    }
}
