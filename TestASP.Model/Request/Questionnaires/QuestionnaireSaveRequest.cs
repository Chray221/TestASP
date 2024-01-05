using System;
using System.ComponentModel.DataAnnotations;
using TestASP.Model.Questionnaires;

namespace TestASP.Model.Request.Questionnaires
{
    public class QuestionnaireSaveRequest : QuestionnaireResponseDto, IValidatableObject
	{
		public List<QuestionSubQuestionSaveRequestDto> Questions { get; set; }
		public QuestionnaireSaveRequest()
		{
		}

        public string GetTempId()
        {
            return $"{Name}-{(Description??Content??"Des").Substring(0,3)}";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
			int count = 0;
            foreach(var question in Questions)
			{
				string questionPropName = $"{nameof(Questions)}[{count++}]";
                switch (question.AnswerTypeId)
                {
                    case Data.Enums.AnswerTypeEnum.BooleanWithSubQuestion:
                        if (question.SubQuestions == null || question.SubQuestions.Count == 0)
                        {
                            yield return new ValidationResult(
                                $"{question.SubQuestions} is required for 'BooleanWithSubQuestion' 'AnswerType'.",
                                new string[]
                                {
                                    $"{questionPropName}.{nameof(question.SubQuestions)}"
                                });
                        }
                        break;
                    case Data.Enums.AnswerTypeEnum.MultipleChoice:
                        if (question.Choices == null || question.Choices.Count == 0)
                        {
                            yield return new ValidationResult(
                                $"{question.Choices} is required for 'MultipleChoice' 'AnswerType'.",
                                new string[]
                                {
                                    $"{questionPropName}.{nameof(question.Choices)}"
                                });
                        }
                        break;
                }
                if (question.SubQuestions?.Count > 0)
                {
                    int subCount = 0;
                    foreach (var subQuestion in question.SubQuestions)
                    {
                        string subQuestionPropName = $"{questionPropName}.{nameof(question.SubQuestions)}[{subCount++}]";
                        switch (subQuestion.AnswerTypeId)
                        {
                            //case Data.Enums.AnswerTypeEnum.BooleanWithSubQuestion:
                            //    if (question.SubQuestions == null || question.SubQuestions.Count == 0)
                            //    {
                            //        yield return new ValidationResult(
                            //            $"{question.SubQuestions} is required for 'BooleanWithSubQuestion' 'AnswerType'.",
                            //            new string[]
                            //            {
                            //        $"{questionPropName}.{nameof(question.SubQuestions)}"
                            //            });
                            //    }
                            //    break;
                            case Data.Enums.AnswerTypeEnum.MultipleChoice:
                                if (question.Choices == null || question.Choices.Count == 0)
                                {
                                    yield return new ValidationResult(
                                        $"{question.Choices} is required for 'MultipleChoice' 'AnswerType'.",
                                        new string[]
                                        {
                                $"{questionPropName}.{nameof(question.Choices)}"
                                        });
                                }
                                break;
                        }
                    }
                }

               
            }
        }
    }

    public class QuestionSubQuestionSaveRequestDto : BaseQuestionResponseDto
    {
		public List<BaseQuestionResponseDto>? SubQuestions { get; set; }
	}
}

