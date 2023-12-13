using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace TestASP.Model.Questionnaires
{
    public class QuestionnaireAnswerSubAnswerRequestDto : QuestionAnswerRequestsDto
    {
        public List<SubQuestionAnswerRequestDto>? SubAnswers { get; set; }

    }
}

