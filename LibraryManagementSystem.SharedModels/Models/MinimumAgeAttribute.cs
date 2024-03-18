using LibraryManagementSystem.SharedModels.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.SharedModels.ValidationAttributes
{
    public class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        public MinimumAgeAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateOfBirth)
            {
                var today = DateTime.Today;
                var age = today.Year - dateOfBirth.Year;
                if (dateOfBirth > today.AddYears(-age))
                    age--;

                if (age < _minimumAge)
                {
                    return new ValidationResult($"The minimum age requirement is {_minimumAge} years.");
                }
            }

            return ValidationResult.Success;
        }
    }

    public class MinimumDurationAttribute : ValidationAttribute
    {
        private readonly int _minimumMonths;

        public MinimumDurationAttribute(int minimumMonths)
        {
            _minimumMonths = minimumMonths;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime endDate && validationContext.ObjectInstance is StudentEnrolmentModel enrolment)
            {
                var startDate = enrolment.StartDate;
                var duration = endDate.Subtract(startDate).Days / 30; // Assuming 30 days per month

                if (duration < _minimumMonths)
                {
                    return new ValidationResult($"The minimum duration between Start Date and End Date is {_minimumMonths} months.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
