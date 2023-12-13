using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestASP.Data
{
    public class QuestionnaireSubAnswer : BaseQuestionnaireAnswer
    {
        public int SubQuestionId { get; set; }
        public int? QuestionnaireAnswerId { get; set; }

        #region ForeignKey
        [ForeignKey(nameof(SubQuestionId))]
        public QuestionnaireSubQuestion? SubQuestion { get; set; }
        [ForeignKey(nameof(QuestionnaireAnswerId))]
        public QuestionnaireAnswer? QuestionnaireAnswer { get; set; }
        #endregion
    }
}

