using System;
namespace TestASP.Model.Questionnaires
{
    public class QuestionnaireAnswerSubAnswerRequestDto : QuestionAnswerRequestsDto
    {
        public List<SubQuestionAnswerRequestDto> SubAnswers { get; set; }
    }
}

