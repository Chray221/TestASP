using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestASP.Data
{
    public class QuestionnaireSubAnswer : BaseQuestionnaireAnswer
    {
        public int SubQuestionId { get; set; }

        #region ForeignKey
        [ForeignKey(nameof(SubQuestionId))]
        public QuestionnaireSubQuestion? SubQuestion { get; set; }
        #endregion
    }
}

