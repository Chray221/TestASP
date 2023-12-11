using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestASP.Data
{
    public class QuestionnaireAnswer : BaseQuestionnaireAnswer
    {
        public int QuestionId { get; set; }

        #region ForeignKey
        [ForeignKey(nameof(QuestionId))]
        public QuestionnaireQuestion? Question { get; set; }
        #endregion
    }
}

