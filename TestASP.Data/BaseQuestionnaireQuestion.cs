using System;
using TestASP.Data.Enums;

namespace TestASP.Data
{
    public abstract class BaseQuestionnaireQuestion : BaseData
    {
        public AnswerTypeEnum AnswerTypeId { get; set; }
        public QuestionTypeEnum QuestionTypeId { get; set; }
        public string Question { get; set; }
        public string Number { get; set; }
    }
}

