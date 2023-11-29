using System.ComponentModel.DataAnnotations;

namespace TestASP.Model.Helpers
{
    public class EqualToValidationAttribute: ValidationAttribute
	{
        public string FieldName { get; set; }
        public EqualToValidationAttribute(string fieldName): base()
		{
            FieldName = fieldName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (validationContext != null)
            {
                var fieldPropertry = validationContext.ObjectInstance.GetType().GetProperty(FieldName);
                if (fieldPropertry == null)
                {
                    return new ValidationResult($"{FieldName} does not exist");
                }
                else if (value != null && !value.Equals(fieldPropertry.GetValue(validationContext.ObjectInstance)))
                {
                    return new ValidationResult(ErrorMessage ?? $"{{0}} is not equals to {FieldName}.");
                }

                //return base.IsValid(value, validationContext);
            }
            return null;
        }

        
    }
}

