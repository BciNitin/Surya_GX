using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MobiVUE.Utility.Validations
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PhoneAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) return ValidationResult.Success;

            string pattern = @"^(\+\s?)?((?<!\+.*)\(\+?\d+([\s\-\.]?\d+)?\)|\d+)([\s\-\.]?(\(\d+([\s\-\.]?\d+)?\)|\d+))*(\s?(x|ext\.?)\s?\d+)?$";
            RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;

            Regex regEx = new Regex(pattern, options);
            if (regEx.Match(value.ToString()).Length > 0)
            {
                return ValidationResult.Success;
            }
            return new System.ComponentModel.DataAnnotations.ValidationResult(ErrorMessage ?? "Invalid phone number.");
        }
    }
}