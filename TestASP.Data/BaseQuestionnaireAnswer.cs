using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestASP.Data
{
    public abstract class BaseQuestionnaireAnswer : BaseData
    {
        public string Answer { get; set; }
        public int? AnswerId { get; set; }

        #region ForeignKey
        [ForeignKey(nameof(AnswerId))]
        public QuestionnaireQuestionChoice? AnswerChoice { get; set; }
        #endregion
    }
}

