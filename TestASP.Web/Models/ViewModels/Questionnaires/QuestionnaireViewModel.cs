using TestASP.Model.Questionnaires;

namespace TestASP.Web.Models.ViewModels.Questionnaires;

public class QuestionnaireViewModel : List<UserQuestionnaireResponseDto>
{
    public QuestionnaireViewModel()
    {
    }

    public QuestionnaireViewModel(IEnumerable<UserQuestionnaireResponseDto> collection) 
        : base(collection)
    {
    }
}
