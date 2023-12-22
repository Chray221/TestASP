
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestASP.Data.Enums;
using TestASP.Model.Questionnaires;
using TestASP.Web.IServices;
using TestASP.Web.Models;
using TestASP.Web.Models.ViewModels.Questionnaires;

namespace TestASP.Web;

[Authorize]
public class QuestionnaireController : BaseController
{
    private IQuestionnaireService _questionnaireService;

    public QuestionnaireController(
        ILogger<QuestionnaireController> logger,
        IQuestionnaireService questionnaireService) 
        : base(logger)
    {
        _questionnaireService = questionnaireService;
    }

    public Task<IActionResult> Index(
        [FromServices] IMapper mapper)
    {
        return TryCatch( async () =>
        {
            return await ApiResult(
                new QuestionnaireViewModel(),
                await _questionnaireService.GetAsync(0),
                async data => 
                {
                    return View(mapper.Map<QuestionnaireViewModel>(data));
                });
        });
    }    

    // [HttpGet("{id:int}/Answer/{userQuestionnaireId:int}")]
    // [HttpGet("{id}/Answer")]
    [HttpGet]
    public Task<IActionResult> AnswerAsync(
        [FromRoute]int id,
        [FromRoute]int? userQuestionnaireId,        
        [FromServices] IMapper mapper)
    {
        return TryCatch( async () =>
        {
            return await ApiResult(
                new QuestionnaireQuestionAnswerViewModel(),
                await _questionnaireService.GetWithQuestionAnswerAsync(0,id,userQuestionnaireId),
                async data => 
                {
                    return View(mapper.Map<QuestionnaireQuestionAnswerViewModel>(data));
                });
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [ActionName("Answer")]
    public Task<IActionResult> SaveOrUpdateAnswerAsync(
        [FromRoute]int id,
        [FromRoute]int? userQuestionnaireId,
        [FromForm] QuestionnaireQuestionAnswerViewModel saveRequest,
        [FromServices] IMapper mapper)
    {
        return TryCatch( async () =>
        {
            if(!ModelState.IsValid)
            {            
                await TryUpdateModelAsync(saveRequest);
                return View(model: saveRequest);
            }
            // update
            if(userQuestionnaireId != null)
            {
                return await ApiResult(
                    new QuestionnaireViewModel(),
                    await _questionnaireService.UpdateAsync(0,id,userQuestionnaireId ?? 0,mapper.Map<List<QuestionnaireAnswerSubAnswerRequestDto>>(saveRequest)),
                    async data => 
                    {
                        // return View("Answer",mapper.Map<QuestionnaireQuestionAnswerViewModel>(data));
                        return RedirectToAction("Index");
                    });
            }
            //save
            return await ApiResult(
                new QuestionnaireViewModel(),
                await _questionnaireService.SaveAsync(0,id,mapper.Map<List<QuestionnaireAnswerSubAnswerRequestDto>>(saveRequest)),
                async data => 
                {
                    // return View("Answer",mapper.Map<QuestionnaireQuestionAnswerViewModel>(data));
                    return RedirectToAction("Index");
                });
        });
    }

    

    [ValidateAntiForgeryToken]
    [HttpGet]
    [ActionName("SubQuestions")]
    public Task<IActionResult> SubQuestionsAsync(
        [FromRoute]string? answer,
        [FromBody] List<SubQuestionAnswerViewModel> subQuestions,
        [FromServices] IMapper mapper)
    {
        return TryCatch( async () =>
        {
            var newSubQuestions = new List<SubQuestionAnswerViewModel>();
            
            if (bool.TryParse(answer, out bool result))
            {
                newSubQuestions = subQuestions.Where(subQuestion =>
                    subQuestion.QuestionTypeId == QuestionTypeEnum.SubQuestion ||
                    subQuestion.QuestionTypeId == (result
                        ? QuestionTypeEnum.BooleanYesSubQuestion // if answer is true
                        : QuestionTypeEnum.BooleanNoSubQuestion)).ToList(); // if answer is false
            }
            else
            {            
                newSubQuestions = subQuestions.Where(subQuestion => subQuestion.QuestionTypeId == QuestionTypeEnum.SubQuestion).ToList();
            }
            
            return PartialView("_AnswerSubQuestions", newSubQuestions);
        });
    }
}
