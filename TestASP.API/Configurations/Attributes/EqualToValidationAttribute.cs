using System;
using System.ComponentModel.DataAnnotations;

namespace TestASP.API.Configurations.Attributes
{
	public class EqualToValidationAttribute: ValidationAttribute
	{
        public string FieldName { get; set; }
        public EqualToValidationAttribute(string fieldName)
		{
            FieldName = fieldName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            return base.IsValid(value, validationContext);
        }
    }
}

