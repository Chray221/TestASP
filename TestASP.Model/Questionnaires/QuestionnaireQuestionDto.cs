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
    public class QuestionnaireQuestionsResponseDto : UserQuestionnaireResponseDto
    {
        public List<QuestionAnswerSubQuestionAnswerResponseDto> QuestionAnswers { get; set; }
    }

}

