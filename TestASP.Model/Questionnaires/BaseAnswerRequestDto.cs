using System;
using System.ComponentModel.DataAnnotations;

namespace TestASP.Model.Questionnaires
{
    public class BaseAnswerRequestDto : BaseDto, IValidatableObject
    {
        public string? Answer { get; set; }
        public int? AnswerId { get; set; }

        public bool HasAnswer()
        {
            return !string.IsNullOrEmpty(Answer) || AnswerId != null;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!HasAnswer())
            {
                yield return new ValidationResult("Answer or AnswerId is required.", new[] { $"{nameof(Answer)}", $"{nameof(AnswerId)}" });
            }
        }
    }
}

