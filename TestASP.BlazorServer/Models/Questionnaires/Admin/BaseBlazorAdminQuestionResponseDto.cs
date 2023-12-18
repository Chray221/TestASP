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
            return new List<ValidationResult>();
        }
    }
}

