using TestASP.Model.Questionnaires;

namespace TestASP.Web.Areas.Admin.Models.Questionnaire;

public class AdminQuestionViewModel: BaseAdminQuestionViewModel
{
    public int? QuestionId { get; set; }
}

public class AdminQuestionSubQuestionViewModel: AdminQuestionViewModel
{
    public List<AdminSubQuestionViewModel> SubQuestions { get; set; }
}