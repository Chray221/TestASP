using System.ComponentModel.DataAnnotations;

namespace TestASP.Model.Helpers
{
    public class EqualToValidationAttribute: ValidationAttribute
	{
        public string PropertyName { get; set; }
        public EqualToValidationAttribute(string propertyName): base()
		{
            PropertyName = propertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (validationContext != null)
            {
                var fieldPropertry = validationContext.ObjectInstance.GetType().GetProperty(PropertyName);
                if (fieldPropertry == null)
                {
                    return new ValidationResult($"{PropertyName} does not exist");
                }
                else if (value != null && !value.Equals(fieldPropertry.GetValue(validationContext.ObjectInstance)))
                {
                    return new ValidationResult(ErrorMessage ?? $"{{0}} is not equals to {PropertyName}.");
                }

                //return base.IsValid(value, validationContext);
            }
            return null;
        }

        
    }
}

