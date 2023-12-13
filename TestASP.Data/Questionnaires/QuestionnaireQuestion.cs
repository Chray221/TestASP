using System.ComponentModel.DataAnnotations.Schema;
using TestASP.Data.Enums;

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

        public IEnumerable<QuestionnaireSubQuestion>? GetSubQuestionsFromAnswer(string answer)
        {
            if (AnswerTypeId == AnswerTypeEnum.BooleanWithSubQuestion)
            {
                string answerTemp = answer;
                if (int.TryParse(answerTemp, out int intValue))
                {
                    answerTemp = intValue == 0 ? bool.FalseString : bool.TrueString;
                }
                if (bool.TryParse(answerTemp, out bool result))
                {
                    return SubQuestions?.Where(subQuestion =>
                        subQuestion.QuestionTypeId == QuestionTypeEnum.SubQuestion ||
                        subQuestion.QuestionTypeId == (result
                            ? QuestionTypeEnum.BooleanYesSubQuestion // if answer is true
                            : QuestionTypeEnum.BooleanNoSubQuestion)).ToList(); // if answer is false
                }
            }
            return SubQuestions?.Where(subQuestion => subQuestion.QuestionTypeId == QuestionTypeEnum.SubQuestion).ToList();
        }
    }
}

