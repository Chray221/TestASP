using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestASP.Data
{
    public class QuestionnaireSubQuestion : BaseQuestionnaireQuestion
    {
        public int QuestionId { get; set; }

        #region ForeignKey
        [ForeignKey(nameof(QuestionId))]
        public QuestionnaireQuestion? QuestionnaireQuestion { get; set; }
        #endregion

    }
}

