using System;
using System.ComponentModel.DataAnnotations;
using BootstrapBlazor.Components;
using TestASP.Common.Extensions;
using TestASP.Data.Enums;
using TestASP.Model.Questionnaires;

namespace TestASP.BlazorServer.Models.Questionnaires.Admin
{
	public class BaseBlazorAdminQuestionResponseDto : BaseQuestionResponseDto , IValidatableObject
    {
        SelectedItem? _answerTypeSelected;
        [Required]
        public SelectedItem? AnswerTypeSelected
        {
            get => _answerTypeSelected;
            set
            {
                _answerTypeSelected = value;
                if (_answerTypeSelected != null)
                {
                    AnswerTypeId = _answerTypeSelected.Value.ToEnum<AnswerTypeEnum>();
                }
            }
        }

        SelectedItem? _questionTypeSelected;
        [Required]
        public SelectedItem? QuestionTypeSelected
        {
            get => _questionTypeSelected;
            set
            {
                _questionTypeSelected = value;
                if (_questionTypeSelected != null)
                {
                    QuestionTypeId = _questionTypeSelected.Value.ToEnum<QuestionTypeEnum>();
                }
            }
        }

        public BaseBlazorAdminQuestionResponseDto()
		{
		}

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //yield return new ValidationResult(nameof(), new[] { nameof() });
            if (string.IsNullOrEmpty(Number))
            {
                yield return new ValidationResult("Number is required", new[] { "Number" });
            }
            if (string.IsNullOrEmpty(Question))
            {
                yield return new ValidationResult("Question is required", new[] { "Question" });
            }
            if (AnswerTypeSelected == null)
            {
                yield return new ValidationResult("AnswerType is required", new[] { "AnswerTypeSelected" });
            }
            if (QuestionTypeSelected == null)
            {
                yield return new ValidationResult("QuestionType is required", new[] { "QuestionTypeSelected" });
            }
            if(AnswerTypeId == AnswerTypeEnum.MultipleChoice)
            {
                if (Choices?.Count > 0)
                {
                    int choiceCount = 0;
                    string choicePropName;
                    foreach (var choice in Choices)
                    {
                        choicePropName = $"{nameof(Choices)}[{choiceCount++}]";
                        foreach (var choiceResult in choice.Validate(validationContext))
                        {
                            //yield return choiceResult;
                            yield return choiceResult.ToParentValidationResult(choicePropName);
                        }
                    }
                }
                else
                {
                    yield return new ValidationResult("Choices is required", new[] { "Choices", "AnswerTypeSelected" });
                }
            }
            

            //return new List<ValidationResult>();
        }
    }
}

