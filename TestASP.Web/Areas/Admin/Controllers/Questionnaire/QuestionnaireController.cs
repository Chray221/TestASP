
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using TestASP.Model.Questionnaires;
using TestASP.Model.Request.Questionnaires;
using TestASP.Web.Areas.Admin.Models.Questionnaire;
using TestASP.Web.IServices;
using TestASP.Web.Services;

namespace TestASP.Web.Areas.Admin.Controllers.Questionnaire;

[Authorize]
[Area("Admin")]
[Route("Admin/Questionnaire")]
public class QuestionnaireController : BaseController
{
    private readonly IQuestionnaireService _questionnaireService;
    private readonly QuestionnaireCacheService _questionnaireCache;
    private readonly IMapper _mapper;

    public QuestionnaireController(
        ILogger<QuestionnaireController> logger,
        IQuestionnaireService questionnaireService,
        QuestionnaireCacheService questionnaireCache,
        IMapper mapper)
        : base(logger)
    {
        _questionnaireService = questionnaireService;
        _questionnaireCache = questionnaireCache;
        _mapper = mapper;
    }

    // public async Task<IActionResult> Index()
    // {
    //     return await ApiResult(
    //         new List<QuestionnaireResponseDto>(), 
    //         await _questionnaireService.GetAdminAsync(),
    //         async data => 
    //         {
    //             var cachedData = await _questionnaireCache.GetQuestionnaires<QuestionnaireResponseDto>();
    //             data.AddRange(cachedData.Select( cd => cd.Value));
    //             return View(data);
    //         });
    // }


    [HttpGet("View/{id}")]
    [ActionName("View")]
    public Task<IActionResult> View(string id)
    {
        return TryCatch( async () => 
        {
            if(int.TryParse(id, out int intId))
            {
                return await ApiResult(
                    new QuestionnaireSaveRequest(), 
                    await _questionnaireService.GetAdminItemAsync(intId),
                    async data => 
                    {
                        return View(data);
                    });
            }
            //get from local
            ViewBag.TempId = id;
            var dataResult = _questionnaireCache.Get(id);
            return View(_mapper.Map<QuestionnaireSaveRequest>(dataResult));
        });
    }

    //Admin/Questionnaire/Add
    [HttpGet("New")]
    [HttpGet("Update/{id}")]
    public Task<IActionResult> New(int? id)
    {
        return TryCatch( async () => 
        {
            if(id != null)
            {
                return await ApiResult(
                    new QuestionnaireSaveRequest(), 
                    await _questionnaireService.GetAdminItemAsync(id.Value),
                    async data => 
                    {
                        return View(_mapper.Map<AdminQuestionnaireQuestionsViewModel>(data));
                    });
            }
            //get from local
            return View(new AdminQuestionnaireQuestionsViewModel());
        });

    }

    //Admin/Questionnaire/Add
    [HttpPost("New")]
    public Task<IActionResult> NewAsync(AdminQuestionnaireQuestionsViewModel newQuestionnaireRequest)
    {
        return TryCatch( async () =>
        {
            if(ModelState.IsValid)
            {
                return await ApiResult(
                    request: newQuestionnaireRequest,
                    apiResult: await _questionnaireService.SaveAdminAsync(_mapper.Map<QuestionnaireSaveRequest>(newQuestionnaireRequest)),
                    async (data) => 
                    {
                        return NavigateTo(RedirectToAction("View",new {id = data.Id}));
                    } 
                );
            }
            return View(newQuestionnaireRequest);
        });
    }

    //Admin/Questionnaire/Add
    [HttpGet("Add")]
    public IActionResult Add()
    {
        return View(new AdminQuestionnaireViewModel());
    }

    //Admin/Questionnaire/Add
    //Admin/Questionnaire/Update/{id}
    [HttpPost("Add")]
    [HttpPut("Update")]
    public async Task<IActionResult> AddAsync(
        AdminQuestionnaireViewModel newQuestionnaire,
        string? id)
    {
        id = id ?? Guid.NewGuid().ToString();
        newQuestionnaire.TempId = id;
        _questionnaireCache.Save(id, _mapper.Map<AdminQuestionnaireQuestionsViewModel>(newQuestionnaire));
        
        // return View(new AdminQuestionnaireViewModel());
        return RedirectToAction("View", new { id });
    }

    //Admin/Questionnaire/Update/{id}
    [HttpGet("Update")]
    public Task<IActionResult> Update(string id)
    {
       return TryCatch( async () =>
       {
            var data = _questionnaireCache.Get(id);
            if(data != null)
            {
                return View(data);
            }
            return View();
       });
    }
}
