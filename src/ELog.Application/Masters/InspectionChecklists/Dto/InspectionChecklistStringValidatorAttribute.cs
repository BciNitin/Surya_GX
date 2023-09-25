using ELog.Core;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ELog.Application.Masters.InspectionChecklists.Dto
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class InspectionChecklistStringValidatorAttribute : ValidationAttribute
    {
        private readonly bool _isValueTag;
        public InspectionChecklistStringValidatorAttribute(bool isValueTag)
        {
            _isValueTag = isValueTag;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var inputValueRequired = validationContext.ObjectType.GetProperty(PMMSConsts.PropertyInputValueRequired);
            var inputValueRequiredValue = inputValueRequired?.GetValue(validationContext.ObjectInstance) as bool?;
            if (inputValueRequiredValue == true)
            {
                bool inValidPipeSeprator = false;
                if (value == null)
                {
                    return new ValidationResult(_isValueTag ? PMMSValidationConst.ValueTagRequired : PMMSValidationConst.AcceptanceValueRequired);
                }
                var splitValues = value?.ToString().Trim().Split("|");
                foreach (var code in splitValues)
                {
                    if (code.Trim()?.Length == 0)
                    {
                        inValidPipeSeprator = true;
                    }
                }
                if ((_isValueTag && value?.ToString().IndexOf("|") < 1) || inValidPipeSeprator)
                {
                    return new ValidationResult(PMMSValidationConst.InvalidPipeSepratedString);
                }
                if (value != null && splitValues.Length != splitValues.Distinct().Count())
                {
                    return new ValidationResult(PMMSValidationConst.UniqueValuesPipeSepratedString);
                }
                if (!_isValueTag)
                {
                    if (inValidPipeSeprator)
                    {
                        return new ValidationResult(PMMSValidationConst.InvalidPipeSepratedString);
                    }
                    var valueTagProperty = validationContext.ObjectType.GetProperty(PMMSConsts.PropertyValueTag);
                    var valueTagPropertyValue = valueTagProperty?.GetValue(validationContext.ObjectInstance) as string;

                    var valueTagArray = valueTagPropertyValue.ToLower().Trim().Split("|").Distinct();
                    var acceptanceValueArray = value.ToString().Trim().Split("|").Distinct();

                    var validAcceptanceValueCount = 0;
                    foreach (var item in acceptanceValueArray)
                    {
                        if (valueTagArray.Contains(item.ToLower()))
                        {
                            validAcceptanceValueCount++;
                        }
                    }

                    if (validAcceptanceValueCount != acceptanceValueArray.Count())
                    {
                        return new ValidationResult(PMMSValidationConst.InvalidAccpetanceValueFromValueTag);
                    }
                }
                return ValidationResult.Success;
            }
            return ValidationResult.Success;
        }
    }
}
