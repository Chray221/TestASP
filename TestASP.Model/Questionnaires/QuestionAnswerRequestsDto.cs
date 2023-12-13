using System;
using System.ComponentModel.DataAnnotations;

namespace TestASP.Model.Questionnaires
{
    public class QuestionAnswerRequestsDto : BaseAnswerRequestDto
    {
        public int QuestionId { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (QuestionId <= 0)
            {
                yield return new ValidationResult("QuestionId is required.", new[] { $"{nameof(QuestionId)}" });
            }
            foreach (var item in base.Validate(validationContext))
            {
                yield return item;
            }
        }
    }
}

