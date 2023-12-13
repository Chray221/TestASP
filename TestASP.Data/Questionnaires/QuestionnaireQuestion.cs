using System.ComponentModel.DataAnnotations.Schema;

namespace TestASP.Data
{
    public class QuestionnaireQuestion : BaseQuestionnaireQuestion
    {
        public int QuestionnaireId { get; set; }

        #region ForeignKey
        [ForeignKey(nameof(QuestionnaireId))]
        public Questionnaire? Questionnaire { get; set; }
        #endregion

        public virtual List<QuestionnaireSubQuestion>? SubQuestions { get; set; }
    }
}

