using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace MobiVUE.Utility.Validations
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CollectionValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var collection = (ICollection)value;
            if (value == null || collection == null) return new ValidationResult(ErrorMessage ?? "Invalid use of attribute");
            if (collection.Count == 0) return new ValidationResult(ErrorMessage ?? "Collection must have items");

            return ValidationResult.Success;
        }
    }
}