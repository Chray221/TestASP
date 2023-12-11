using System;
using System.ComponentModel.DataAnnotations.Schema;
using TestASP.Data.Enums;

namespace TestASP.Data
{
    public class QuestionnaireQuestion : BaseQuestionnaireQuestion
    {
        public int QuestionnaireId { get; set; }
        public AnswerTypeEnum AnswerTypeId { get; set; }
        public QuestionTypeEnum QuestionTypeId { get; set; }
        public string Question { get; set; }
        public string Number { get; set; }

        #region ForeignKey
        [ForeignKey(nameof(QuestionnaireId))]
        public QuestionnaireQuestion? Questionnaire { get; set; }
        #endregion

        public virtual List<QuestionnaireSubQuestion> SubQuestions { get; set; }
    }
}

