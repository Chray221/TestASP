using System;
using System.ComponentModel.DataAnnotations;

namespace TestASP.Model.Questionnaires
{
    public class QuestionChoiceDto : BaseDto , IValidatableObject
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(string.IsNullOrEmpty(Name))
            {
                yield return new ValidationResult("Name is required", new[] { "Name" });
            }
            if (string.IsNullOrEmpty(Value))
            {
                yield return new ValidationResult("Value is required", new[] { "Value" });
            }
        }
    }
}

