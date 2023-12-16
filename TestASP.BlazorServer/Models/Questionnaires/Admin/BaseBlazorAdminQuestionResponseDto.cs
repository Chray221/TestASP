using System;
using System.ComponentModel.DataAnnotations;
using TestASP.Model.Questionnaires;

namespace TestASP.BlazorServer.Models.Questionnaires.Admin
{
	public class BaseBlazorAdminQuestionResponseDto : BaseQuestionResponseDto , IValidatableObject
    {
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

