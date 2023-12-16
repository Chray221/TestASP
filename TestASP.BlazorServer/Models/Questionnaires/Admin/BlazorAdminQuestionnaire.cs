using System.ComponentModel.DataAnnotations;
using TestASP.BlazorServer.Components;
using TestASP.Model.Questionnaires;

namespace TestASP.BlazorServer.Models.Questionnaires.Admin;

public class BlazorAdminQuestionnaire : QuestionnaireResponseDto, IValidatableObject
{
	public List<BlazorAdminQuestion> Questions { get; set; }
	public BlazorAdminQuestionnaire()
	{
	}

	public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(Questions?.Count > 0)
        {
            int qaCount = 0;
            string qaPropName = "";
            foreach (var questionAnswer in Questions)
            {
                qaPropName = $"{nameof(Questions)}[{qaCount++}]";
                //qaPropName = $"{nameof(QuestionAnswers)}";
                foreach (ValidationResult qaValidationResult in questionAnswer.Validate(validationContext))
                {
                    yield return new ValidationResult(
                                        qaValidationResult.ErrorMessage,
                                        qaValidationResult.MemberNames.Select( memName => $"{qaPropName}.{memName}"  ));
                }
            }
        }
	}
}

