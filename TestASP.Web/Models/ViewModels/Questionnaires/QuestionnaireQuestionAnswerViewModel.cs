using System.ComponentModel.DataAnnotations;
using TestASP.Data.Enums;
using TestASP.Model.Questionnaires;

namespace TestASP.Web.Models.ViewModels.Questionnaires;

public class QuestionnaireQuestionAnswerViewModel : UserQuestionnaireResponseDto, IValidatableObject
{
    public List<QuestionAnswerViewModel> QuestionAnswers { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {            
        if(QuestionAnswers != null && QuestionAnswers.Count > 0)
        {
            int qaCount = 0;
            string qaPropName = "";
            foreach (var questionAnswer in QuestionAnswers)
            {
                qaPropName = $"{nameof(QuestionAnswers)}[{qaCount++}]";
                foreach (ValidationResult qaValidationResult in questionAnswer.Validate(validationContext))
                {
                    yield return new ValidationResult(
                                        qaValidationResult.ErrorMessage,
                                        qaValidationResult.MemberNames.Select( memName => $"{qaPropName}.{memName}"  ));
                }
            }
        }
    }
}

public class QuestionAnswerViewModel: BaseQuestionAnswerViewModel
{
    public int QuestionId { get; set; }
    public List<SubQuestionAnswerViewModel>? SubQuestionAnswers { get; set; }
    public List<SubQuestionAnswerViewModel>? SubQuestions 
    { 
        get 
        {
            return SubQuestionAnswers?.Where(subQuestion => subQuestion.QuestionTypeId == QuestionTypeEnum.SubQuestion).ToList();
        }
    }
    public List<SubQuestionAnswerViewModel>? YesSubQuestions => SubQuestionAnswers?.Where(subQuestion => subQuestion.QuestionTypeId == QuestionTypeEnum.BooleanYesSubQuestion).ToList();
    public List<SubQuestionAnswerViewModel>? NoSubQuestions  => SubQuestionAnswers?.Where(subQuestion => subQuestion.QuestionTypeId == QuestionTypeEnum.BooleanNoSubQuestion).ToList();
    IList<SubQuestionAnswerViewModel>? _answeredSubQuestions = new  List<SubQuestionAnswerViewModel>();
    string? prevAnswer;
    // [ValidateComplexType]
    public IList<SubQuestionAnswerViewModel>? AnsweredSubQuestions
    {
        get 
        {            
            if (SubQuestionAnswers == null)
            {
                return new List<SubQuestionAnswerViewModel>();
            }

            bool isSameAnswer = prevAnswer == Answer;
            if(!string.IsNullOrEmpty(prevAnswer) && isSameAnswer)
            {
                return _answeredSubQuestions;
            }

            if (AnswerTypeId == AnswerTypeEnum.BooleanWithSubQuestion)
            {
                string? answer = Answer;
                if (int.TryParse(Answer, out int intValue))
                {
                    answer = intValue == 0 ? bool.FalseString : bool.TrueString;
                }
                if (bool.TryParse(answer, out bool result))
                {
                    _answeredSubQuestions = SubQuestionAnswers.Where(subQuestion =>
                        subQuestion.QuestionTypeId == QuestionTypeEnum.SubQuestion ||
                        subQuestion.QuestionTypeId == (result
                            ? QuestionTypeEnum.BooleanYesSubQuestion // if answer is true
                            : QuestionTypeEnum.BooleanNoSubQuestion)).ToList(); // if answer is false
                    return _answeredSubQuestions;
                }
            }
            _answeredSubQuestions = SubQuestions;

            return _answeredSubQuestions;
            
        }
    }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var results in base.Validate(validationContext))
        {
            yield return results;
        }
        if (AnsweredSubQuestions != null && AnsweredSubQuestions.Count() > 0)
        {
            int sqaCount = 0;
            string sqaPropName = "";
            foreach (SubQuestionAnswerViewModel subQuestionAnswer in AnsweredSubQuestions)
            {
                int subQuestionIndex = SubQuestionAnswers?.FindIndex( sub => sub.SubQuestionId == subQuestionAnswer.SubQuestionId) ?? 0;
                sqaPropName = $"{nameof(SubQuestionAnswers)}[{subQuestionIndex}]";
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

public class SubQuestionAnswerViewModel: BaseQuestionAnswerViewModel
{
    public int SubQuestionId { get; set; }
}

public class QuestionChoiceViewModel: QuestionChoiceDto
{

}

#region Base Models
public class BaseAnswerViewModel
{    
    public string? Answer { get; set; }
    public int? AnswerId { get; set; }

    public bool HasNoAnswer()
    {
        return string.IsNullOrEmpty(Answer) /*&& AnswerDate == null*/ && (AnswerId ?? 0) <= 0;
    }

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (HasNoAnswer())
        {
            yield return new ValidationResult($"Answer is Required.", new[] { nameof(Answer) });
            // yield return new ValidationResult($"AnswerDate isRequired.", new[] { nameof(AnswerDate) });
            yield return new ValidationResult($"AnswerId isRequired.", new[] { nameof(AnswerId) });
            // yield return new ValidationResult($"AnswerChoice isRequired.", new[] { nameof(AnswerChoice) });
            // yield return new ValidationResult($"AnswerCheckbox isRequired.", new[] { nameof(AnswerCheckbox) });
        }
    }

    // public static bool HasNoAnswerRule(BootStrapQuestionAnswerBaseResponseDto item)
    // {
    //     return item.HasNoAnswer();
    // }
}

public class BaseQuestionAnswerViewModel : BaseAnswerViewModel
{
    // Question
    public AnswerTypeEnum AnswerTypeId { get; set; }
    public QuestionTypeEnum QuestionTypeId { get; set; }
    public string Question { get; set; }
    public string Number { get; set; }

    public List<QuestionChoiceViewModel>? Choices { get; set; }

    public string GetBooleanClass => QuestionTypeId switch {
        QuestionTypeEnum.BooleanYesSubQuestion => "Yes",
        QuestionTypeEnum.BooleanNoSubQuestion => "No",
        _ => "Sub"
    };


    // IEnumerable<SelectedItem>? _choices;
    // public IEnumerable<SelectedItem>? GetChoices()
    // {
    //     switch (AnswerTypeId)
    //     {
    //         case AnswerTypeEnum.Boolean:
    //         case AnswerTypeEnum.BooleanWithSubQuestion:
    //             _choices = _choices ?? new List<SelectedItem>()
    //             {
    //                 new SelectedItem("True", "Yes") {  Active = Answer == "True"},
    //                 new SelectedItem("False", "No") { Active = Answer == "False"}
    //             };
    //             break;
    //         case AnswerTypeEnum.MultipleChoice:
    //             _choices = _choices ?? Choices?.Select(choice =>
    //                 new SelectedItem($"{choice.Id}", $"{choice.Value} - {choice.Name}") {  Active = choice.Id == AnswerId}).ToList();
    //             break;
    //         default:
    //             return null;
    //     }
    //     return _choices;
    // }

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
                        AnswerId = null;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
#endregion
