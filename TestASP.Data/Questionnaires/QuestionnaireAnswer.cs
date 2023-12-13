using System;
using System.ComponentModel.DataAnnotations.Schema;
using TestASP.Data.Questionnaires;

namespace TestASP.Data
{
    public class QuestionnaireAnswer : BaseQuestionnaireAnswer
    {
        public int QuestionId { get; set; }
        public int UserQuestionnaireId { get; set; }

        #region ForeignKey
        [ForeignKey(nameof(QuestionId))]
        public QuestionnaireQuestion? Question { get; set; }
        [ForeignKey(nameof(UserQuestionnaireId))]
        public UserQuestionnaire? UserQuestionnaire { get; set; }
        #endregion

        public List<QuestionnaireSubAnswer>? SubAnswers { get; set; }
    }
}

