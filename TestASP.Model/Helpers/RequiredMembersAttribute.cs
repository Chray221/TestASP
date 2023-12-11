using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace TestASP.Model.Helpers
{
    public class CustomActionValidationAttribute : CustomActionValidationAttribute<object>
    {
        public CustomActionValidationAttribute(Func<object, bool> validation) : base(validation)
        {
        }
    }

    public class  CustomActionValidationAttribute<T> : ValidationAttribute
    {
        private Func<T, bool> Validation { get; }
        public bool IsRequired { get; set; }

        public CustomActionValidationAttribute(Func<T, bool> validation) : base()
        {
            Validation = validation;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (validationContext != null)
            {
                if(validationContext.ObjectInstance is T data)
                {
                    if (Validation.Invoke(data))
                    {
                        return new ValidationResult(ErrorMessage ?? $"{{0}} is required.");
                    }
                }
                else if(IsRequired)
                {
                    return new ValidationResult($"{{0}} is required.");
                }

                //return base.IsValid(value, validationContext);
            }
            return null;
        }
    }
    public class EitherRequiredAttribute : ValidationAttribute
    {
        private string[] PropertyNames;
        public EitherRequiredAttribute(params string[] propertyNames): base()
        {
            PropertyNames = propertyNames;
        }

        public override bool IsValid(object? value)
        {
            return base.IsValid(value);
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (validationContext != null)
            {
                // if value is null
                if(value == null || value == default)
                {
                    // and other property is also null
                    if(PropertyNames != null && PropertyNames.Length > 0)
                    {
                        // if all is null
                        bool isAllNull = PropertyNames.All(propName =>
                        {
                            var propertryInfo = validationContext.ObjectInstance.GetType().GetProperty(propName);
                            object? propertyValue = propertryInfo?.GetValue(validationContext.ObjectInstance);
                            if (propertyValue != null && propertyValue != default)
                            {
                                return false;
                            }
                            return true;
                        });
                        if (!isAllNull)
                        {
                            return null;
                        }
                    }
                    return new ValidationResult(ErrorMessage ?? $"{{0}} is Required.");
                }
                //return base.IsValid(value, validationContext);
            }
            return null;
            //return base.IsValid(value, validationContext);
        }
    }

    public class RequiredMembersAttribute : ValidationAttribute
	{
        
        public string PropertyName { get; set; }
        public RequiredMembersAttribute(string propertyName) : base()
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

