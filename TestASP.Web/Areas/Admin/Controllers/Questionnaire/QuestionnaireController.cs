
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestASP.Model.Questionnaires;
using TestASP.Model.Request.Questionnaires;
using TestASP.Web.Areas.Admin.Models.Questionnaire;
using TestASP.Web.IServices;

namespace TestASP.Web.Areas.Admin.Controllers.Questionnaire;

[Authorize]
[Area("Admin")]
[Route("Admin/Questionnaire")]
public class QuestionnaireController : BaseController
{
    private readonly IQuestionnaireService _questionnaireService;
    public QuestionnaireController(
        ILogger<QuestionnaireController> logger,
        IQuestionnaireService questionnaireService) 
        : base(logger)
    {
        _questionnaireService = questionnaireService;
    }

    public async Task<IActionResult> Index()
    {
        return await ApiResult(
            new List<QuestionnaireResponseDto>(), 
            await _questionnaireService.GetAdminAsync(),
            async data => 
            {
                return View(data);
            });
    }


    [HttpGet("View")]
    [ActionName("View")]
    public async Task<IActionResult> View(int id)
    {
        return await ApiResult(
            new QuestionnaireSaveRequest(), 
            await _questionnaireService.GetAdminItemAsync(id),
            async data => 
            {
                return View(data);
            });
    }

    [HttpGet("Add")]
    [ActionName("Add")]
    public IActionResult Add()
    {
        // return View("Add",new AdminQuestionnaireViewModel());
        return View(new AdminQuestionnaireViewModel());
    }

    [HttpGet("AddQuestion")]
    [ActionName("AddQuestion")]
    public IActionResult AddQuestion(int id)
    {
        return View(new AdminQuestionViewModel());
    }

    [HttpGet("AddSubQuestion")]
    [ActionName("AddSubQuestion")]
    public IActionResult AddSubQuestion(int id)
    {
        return View(new AdminSubQuestionViewModel());
    }
}
