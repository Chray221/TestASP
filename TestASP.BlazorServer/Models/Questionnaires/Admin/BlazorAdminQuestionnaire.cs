﻿using System.ComponentModel.DataAnnotations;
using TestASP.BlazorServer.Components;
using TestASP.Model.Questionnaires;

namespace TestASP.BlazorServer.Models.Questionnaires.Admin;

public class BlazorAdminQuestionnaire : QuestionnaireResponseDto, IValidatableObject
{
    [ValidateComplexType]
    public List<BlazorAdminQuestion> Questions { get; set; }
    public BlazorAdminQuestionnaire()
	{
	}

	public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(Name))
        {
            yield return new ValidationResult("Name is required", new[] { "Name" });
        }
        if (string.IsNullOrEmpty(Description))
        {
            yield return new ValidationResult("Description is required", new[] { "Description" });
        }
        if (Questions?.Count > 0)
        {
            int qCount = 0;
            string qPropName = "";
            foreach (BlazorAdminQuestion question in Questions)
            {
                qPropName = $"{nameof(Questions)}[{qCount++}]";
                //qaPropName = $"{nameof(QuestionAnswers)}";
                foreach (ValidationResult qaValidationResult in question.Validate(validationContext))
                {
                    yield return new ValidationResult(
                                        qaValidationResult.ErrorMessage,
                                        qaValidationResult.MemberNames.Select(memName => $"{qPropName}.{memName}"));
                }
            }
        }
	}
}

