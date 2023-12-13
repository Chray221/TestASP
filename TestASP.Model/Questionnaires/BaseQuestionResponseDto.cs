using System;
using TestASP.Data.Enums;

namespace TestASP.Model.Questionnaires
{
    public class BaseQuestionResponseDto : BaseDto
    {
        // Question
        public AnswerTypeEnum AnswerTypeId { get; set; }
        public QuestionTypeEnum QuestionTypeId { get; set; }
        public string Question { get; set; }
        public string Number { get; set; }

        public List<QuestionChoiceDto>? Choices { get; set; }
    }
}

