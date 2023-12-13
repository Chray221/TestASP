using System;
using TestASP.Data.Enums;
namespace TestASP.Model.Questionnaires
{
    public class QuestionAnswerSubQuestionAnswerResponseDto : QuestionAnswerResponseDto
    {
        public List<SubQuestionAnswerResponseDto>? SubQuestionAnswers { get; set; }

        public IEnumerable<SubQuestionAnswerResponseDto> GetBooleanSubQuestions()
        {
            if (AnswerTypeId == AnswerTypeEnum.Boolean)
            {
                string answer = Answer;
                if (int.TryParse(Answer, out int intValue))
                {
                    answer = intValue == 0 ? bool.FalseString : bool.TrueString;
                }
                if (bool.TryParse(answer, out bool result))
                {
                    return SubQuestionAnswers.Where(subQuestion =>
                        subQuestion.QuestionTypeId == (result
                            ? QuestionTypeEnum.BooleanYesSubQuestion // if answer is true
                            : QuestionTypeEnum.BooleanNoSubQuestion)); // if answer is false
                }
            }
            return null;
        }
    }
}

