using System;
using System.ComponentModel.DataAnnotations;

namespace MobiVUE.Utility.Validations
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RequiredButNotDefaultAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || (value.GetType().IsValueType && value.Equals(Activator.CreateInstance(value.GetType()))))
            {
                return new System.ComponentModel.DataAnnotations.ValidationResult(ErrorMessage ?? "Invalid value.");
            }
            return ValidationResult.Success;
        }
    }
}