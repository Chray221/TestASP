using System;
using System.ComponentModel.DataAnnotations;
using TestASP.Common.Extensions;
using TestASP.Model;
using TestASP.Model.Questionnaires;

namespace TestASP.BlazorServer.Models.Questionnaires.Admin
{
    public class BlazorAdminQuestion : BaseBlazorAdminQuestionResponseDto
    {
        public List<BlazorAdminSubQuestion>? SubQuestions { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var item in base.Validate(validationContext))
            {
                yield return item;
            }
            if (SubQuestions?.Count > 0)
            {
                int sqCount = 0;
                string sqPropName ;
                foreach (BlazorAdminSubQuestion subQuestion in SubQuestions)
                {
                    sqPropName = $"{nameof(SubQuestions)}[{sqCount++}]";
                    foreach (ValidationResult subItemResult in subQuestion.Validate(validationContext))
                    {
                        //yield return subItemResult;
                        yield return subItemResult.ToParentValidationResult(sqPropName);
                    }
                }
            }
        }
    }
}

