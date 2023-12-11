using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using TestASP.Common.Extensions;
using TestASP.Data.Enums;
using TestASP.Model.Helpers;
using TestASP.Model.Questionnaires;

namespace TestASP.BlazorServer.Models
{
    public class BootStrapQuestionAnswerBaseResponseDto : IValidatableObject
    {
        // Question
        public AnswerTypeEnum AnswerTypeId { get; set; }
        public QuestionTypeEnum QuestionTypeId { get; set; }
        public string Question { get; set; }
        public string Number { get; set; }
        // Question Answer
        [EitherRequired(nameof(AnswerId))]
        public string? Answer { get; set; }
        public int? AnswerId { get; set; }

        DateTime? _answerDate;
        public DateTime? AnswerDate
        {
            get { return _answerDate; }
            set { _answerDate = value; Answer = _answerDate?.ToString("MMM dd, yyyy"); }
        }

        public List<QuestionChoiceDto> Choices { get; set; }

        SelectedItem? _answerChoice;
        public SelectedItem? AnswerChoice
        {
            get { return _answerChoice; }
            set { _answerChoice = value; SetAnswer(_answerChoice?.Value); }
        }

        string? _answerCheckbox;
        public string? AnswerCheckbox
        {
            get { return _answerCheckbox; }
            set {  _answerCheckbox = string.IsNullOrEmpty(_answerCheckbox) ? value : value?.Replace(_answerCheckbox ?? "", "").Replace(",","") ?? "";
                SetAnswer(_answerCheckbox);
            }
        }

        IEnumerable<SelectedItem>? _choices;
        public IEnumerable<SelectedItem>? GetChoices()
        {
            switch (AnswerTypeId)
            {
                case AnswerTypeEnum.Boolean:
                case AnswerTypeEnum.BooleanWithSubQuestion:
                    _choices = _choices ?? new List<SelectedItem>()
                    {
                        new SelectedItem("True", "Yes"),
                        new SelectedItem("False", "No")
                    };
                    break;
                case AnswerTypeEnum.MultipleChoice:
                    _choices = _choices ?? Choices?.Select(choice =>
                    new SelectedItem($"{choice.Id}", $"{choice.Value} - {choice.Name}")).ToList();
                    break;
                default:
                    return null;
            }
            return _choices;
        }

        public void SetAnswer(string? value)
        {
            if (value != null)
            {
                switch (AnswerTypeId)
                {
                    case AnswerTypeEnum.Boolean:
                    case AnswerTypeEnum.BooleanWithSubQuestion:
                        Answer = value;
                        break;
                    case AnswerTypeEnum.MultipleChoice:
                        if (int.TryParse(value, out int answerId))
                        {
                            AnswerId = answerId;
                        }
                        else
                        {
                            AnswerId = 0;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public bool HasNoAnswer()
        {
            return string.IsNullOrEmpty(Answer) && AnswerDate == null && AnswerId <= 0;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (HasNoAnswer())
            {
                yield return new ValidationResult($"Answer is Required.", new[] { nameof(Answer) });
                yield return new ValidationResult($"AnswerDate isRequired.", new[] { nameof(AnswerDate) });
                yield return new ValidationResult($"AnswerId isRequired.", new[] { nameof(AnswerId) });
                yield return new ValidationResult($"AnswerChoice isRequired.", new[] { nameof(AnswerChoice) });
                yield return new ValidationResult($"AnswerCheckbox isRequired.", new[] { nameof(AnswerCheckbox) });
            }
        }
        public static bool HasNoAnswerRule(BootStrapQuestionAnswerBaseResponseDto item)
        {
            return item.HasNoAnswer();
        }
    }

    public class BootStrapQuestionAnswerResponseDto : BootStrapQuestionAnswerBaseResponseDto
    {
        // Question
        [Required]
        public int QuestionId { get; set; }
    }

    public class BootStrapSubQuestionAnswerResponseDto : BootStrapQuestionAnswerBaseResponseDto
    {
        // SubQuestion
        [Required]
        public int SubQuestionId { get; set; }
    }

    [ComplexType]
    public class BootStrapQuestionAnswerSubQuestionAnswerResponseDto : BootStrapQuestionAnswerResponseDto
    {
        public List<BootStrapSubQuestionAnswerResponseDto> SubQuestionAnswers { get; set; }

        IList<BootStrapSubQuestionAnswerResponseDto> _subQuestions = new  List<BootStrapSubQuestionAnswerResponseDto>();
        string? prevAnswer;
        [ValidateComplexType]
        public IList<BootStrapSubQuestionAnswerResponseDto> SubQuestions
        {
            get
            {
                if (SubQuestionAnswers == null)
                {
                    return new List<BootStrapSubQuestionAnswerResponseDto>();
                }

                bool isSameAnswer = prevAnswer == Answer;
                if(!string.IsNullOrEmpty(prevAnswer) && isSameAnswer)
                {
                    return _subQuestions;
                }

                if (AnswerTypeId == AnswerTypeEnum.BooleanWithSubQuestion)
                {
                    string answer = Answer;
                    if (int.TryParse(Answer, out int intValue))
                    {
                        answer = intValue == 0 ? bool.FalseString : bool.TrueString;
                    }
                    if (bool.TryParse(answer, out bool result))
                    {
                        _subQuestions = SubQuestionAnswers.Where(subQuestion =>
                            subQuestion.QuestionTypeId == QuestionTypeEnum.SubQuestion ||
                            subQuestion.QuestionTypeId == (result
                                ? QuestionTypeEnum.BooleanYesSubQuestion // if answer is true
                                : QuestionTypeEnum.BooleanNoSubQuestion)).ToList(); // if answer is false
                        return _subQuestions;
                    }
                }
                _subQuestions = SubQuestionAnswers.Where(subQuestion => subQuestion.QuestionTypeId == QuestionTypeEnum.SubQuestion).ToList();

                return _subQuestions;
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var results in base.Validate(validationContext))
            {
                yield return results;
            }
            if (SubQuestions != null && SubQuestions.Count() > 0)
            {
                int sqaCount = 0;
                string sqaPropName = "";
                foreach (BootStrapSubQuestionAnswerResponseDto subQuestionAnswer in SubQuestions)
                {
                    sqaPropName = $"{nameof(SubQuestions)}[{sqaCount++}]";
                    //sqaPropName = $"{qaPropName}.{nameof(questionAnswer.GetSubQuestions)}";
                    foreach (ValidationResult sqavalidationResult in subQuestionAnswer.Validate(validationContext))
                    {
                        yield return new ValidationResult(
                                            sqavalidationResult.ErrorMessage,
                                            sqavalidationResult.MemberNames.Select(memName => $"{sqaPropName}.{memName}"));
                    }
                }
            }
        }
    }

    [ComplexType]
    public class BootStrapQuestionnaireQuestionsResponseDto : QuestionnaireResponseDto , IValidatableObject
    {
        [ValidateComplexType]
        public List<BootStrapQuestionAnswerSubQuestionAnswerResponseDto> QuestionAnswers { get; set; }
        public BootStrapQuestionnaireQuestionsResponseDto()
		{
		}

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {            
            if(QuestionAnswers != null && QuestionAnswers.Count > 0)
            {
                int qaCount = 0;
                string qaPropName = "";
                foreach (BootStrapQuestionAnswerSubQuestionAnswerResponseDto questionAnswer in QuestionAnswers)
                {
                    qaPropName = $"{nameof(QuestionAnswers)}[{qaCount++}]";
                    //qaPropName = $"{nameof(QuestionAnswers)}";
                    foreach (ValidationResult qaValidationResult in questionAnswer.Validate(validationContext))
                    {
                        yield return new ValidationResult(
                                            qaValidationResult.ErrorMessage,
                                            qaValidationResult.MemberNames.Select( memName => $"{qaPropName}.{memName}"  ));
                    }
                    //if(questionAnswer.SubQuestions != null && questionAnswer.SubQuestions.Count() > 0)
                    //{
                    //    int sqaCount = 0;
                    //    string sqaPropName = "";
                    //    foreach (BootStrapSubQuestionAnswerResponseDto subQuestionAnswer in questionAnswer.SubQuestions)
                    //    {
                    //        sqaPropName = $"{qaPropName}.{nameof(questionAnswer.SubQuestions)}[{sqaCount++}]";
                    //        //sqaPropName = $"{qaPropName}.{nameof(questionAnswer.GetSubQuestions)}";
                    //        foreach (ValidationResult sqavalidationResult in subQuestionAnswer.Validate(validationContext))
                    //        {
                    //            yield return new ValidationResult(
                    //                                sqavalidationResult.ErrorMessage,
                    //                                sqavalidationResult.MemberNames.Select(memName => $"{sqaPropName}.{memName}"));
                    //        }
                    //    }
                    //}
                }
            }
        }
    }
}

