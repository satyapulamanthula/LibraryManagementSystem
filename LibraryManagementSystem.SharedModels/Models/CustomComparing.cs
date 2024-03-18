using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.SharedModels
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CustomComparing : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var endDateProperty = validationContext.ObjectType.GetProperty("EndDate");
            var endDateValue = endDateProperty?.GetValue(validationContext.ObjectInstance, null) as DateTime?;

            var startDate = (DateTime)value;

            // Check if EndDate is greater than StartDate
            if (endDateValue.HasValue && startDate >= endDateValue.Value)
            {
                return new ValidationResult("End Date must be greater than Start Date.");
            }

            // Ensure that the difference is at least 6 months
            if (endDateValue.HasValue && (endDateValue.Value - startDate).TotalDays < 180)
            {
                return new ValidationResult("End Date must be at least 6 months greater than Start Date.");
            }

            return ValidationResult.Success;
        }

    }
}
