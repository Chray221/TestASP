using System;
using TestASP.Data.Enums;

namespace TestASP.Model.Questionnaires
{
    public class BaseQuestionAnswerResponseDto : BaseQuestionResponseDto
    {
        // Question Answer
        public string? Answer { get; set; }
        public int? AnswerId { get; set; }
    }
}

