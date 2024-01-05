using TestASP.Model.Questionnaires;

namespace TestASP.Web.Areas.Admin.Models.Questionnaire;

public class AdminQuestionnaireViewModel : QuestionnaireResponseDto
{
        public string? TempId {get;set;} = "";
}
