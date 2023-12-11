using System;
using System.ComponentModel.DataAnnotations;
using TestASP.Data.Enums;

namespace TestASP.Model.Questionnaires
{
    /*
    request:{
        questionAnswers: [ {
            questionId: 1,
            answer: true,
            subAnswers: [{
                subQuestionId: 1
                answer: answerHere
            }]
        }]
    }
     */
    #region Request
    public class BaseAnswerRequestDto : BaseDto
    {
        public string Answer { get; set; }
        public int AnswerId { get; set; }
    }

    public class QuestionAnswerRequestsDto : BaseAnswerRequestDto
    {
        public int QuestionId { get; set; }
    }

    public class SubQuestionAnswerRequestDto : BaseAnswerRequestDto
    {
        public int SubQuestionId { get; set; }
    }

    public class QuestionnaireAnswerSubAnswerRequestDto : QuestionAnswerRequestsDto
    {
        public List<SubQuestionAnswerRequestDto> SubAnswers { get; set; }
    }
    #endregion

    /*
    response: {
        id: 1,
        name: Why questionnaires,
        description: Why is this questionnaire asked?,
        questionAnswers:[ {
            questionId : 1,
            answerTypeId: 1,
            questionTypeId: 1
            question: Why?,
            number: 1.
            answer: true,
            subQuestionAnswers: [{          
                subQuestionId : 1 ,
                answerTypeId: 1,
                questionTypeId: 1
                question: Why?,
                number: 1.a,
                answer: true
            }]
        }]
    }    
    */
    #region Response

    public class QuestionChoiceDto: BaseDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class QuestionAnswerBaseResponseDto
    {
        // Question
        public AnswerTypeEnum AnswerTypeId { get; set; }
        public QuestionTypeEnum QuestionTypeId { get; set; }
        public string Question { get; set; }
        public string Number { get; set; }
        // Question Answer
        public string? Answer { get; set; }
        public int? AnswerId { get; set; }

        public List<QuestionChoiceDto> Choices { get; set; }
    }

    public class QuestionAnswerResponseDto : QuestionAnswerBaseResponseDto
    {
        // Question
        public int QuestionId { get; set; }
    }


    public class SubQuestionAnswerResponseDto : QuestionAnswerBaseResponseDto
    {
        // SubQuestion
        public int SubQuestionId { get; set; }
    }

    public class QuestionAnswerSubQuestionAnswerResponseDto : QuestionAnswerResponseDto
    {
        public List<SubQuestionAnswerResponseDto> SubQuestionAnswers { get; set; }

        public IEnumerable<SubQuestionAnswerResponseDto> GetBooleanSubQuestions()
        {
            if (AnswerTypeId == AnswerTypeEnum.Boolean)
            {
                string answer = Answer;
                if(int.TryParse(Answer, out int intValue))
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

    public class QuestionnaireResponseDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class QuestionnaireQuestionsResponseDto : QuestionnaireResponseDto
    {
        public List<QuestionAnswerSubQuestionAnswerResponseDto> QuestionAnswers { get; set; }
    }
    #endregion

}

