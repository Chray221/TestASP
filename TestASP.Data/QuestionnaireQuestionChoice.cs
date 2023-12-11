using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestASP.Data
{
    public class QuestionnaireQuestionChoice : BaseData
    {
        public int? QuestionId { get; set; }
        public int? SubQuestionId { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }

        #region ForeignKey
        [ForeignKey(nameof(QuestionId))]
        public QuestionnaireQuestion? Question { get; set; }
        [ForeignKey(nameof(SubQuestionId))]
        public QuestionnaireSubQuestion? SubQuestion { get; set; }
        #endregion
    }
}

