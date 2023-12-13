using System;
using System.ComponentModel.DataAnnotations;

namespace TestASP.Model.Questionnaires
{
    public class SubQuestionAnswerRequestDto : BaseAnswerRequestDto
    {
        public int SubQuestionId { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SubQuestionId <= 0)
            {
                yield return new ValidationResult("SubQuestionId is required.", new[] { $"{nameof(SubQuestionId)}" });
            }
            foreach (var result in base.Validate(validationContext)) { yield return result; }
        }
    }
}

