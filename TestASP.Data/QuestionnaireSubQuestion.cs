using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestASP.Data
{
    public class QuestionnaireSubQuestion : BaseQuestionnaireQuestion
    {
        public int QuestionId { get; set; }

        #region ForeignKey
        [ForeignKey(nameof(QuestionId))]
        public QuestionnaireQuestion? Question { get; set; }
        #endregion

        public virtual List<QuestionnaireQuestionChoice> Choices { get; set; }
    }
}

