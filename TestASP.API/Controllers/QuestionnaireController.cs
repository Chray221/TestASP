using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TestASP.API.Extensions;
using TestASP.API.Helpers;
using TestASP.Core.IRepository;
using TestASP.Data;
using TestASP.Model;
using TestASP.Model.Questionnaires;
using TestASP.Model.Request.Questionnaires;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestASP.API.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiversion}/api/[controller]")]
    public class QuestionnaireController
        : MyRepositoryControllerBase<IQuestionnaireRepository,Questionnaire>
    {
        public QuestionnaireController(
            IWebHostEnvironment environment,
            IQuestionnaireRepository repository,
            IMapper mapper)
            : base(environment, repository, mapper)
        {
        }

        [Authorize]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(DataResult<List<QuestionnaireResponseDto>>))]
        [HttpGet]
        // GET: api/values
        public async Task<IActionResult> GetAsync()
        {
            List<Questionnaire> questionnaires = await _repository.GetAsync();
            if(questionnaires == null )
            {
                MessageHelper.InternalServerError("Something went wrong in retrieving questionnaires.");
            }
            return MessageHelper.Ok(
                questionnaires?.Select( _mapper.Map<QuestionnaireResponseDto>),
                "Successfully retrieved questionnaires");
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(DataResult<QuestionnaireSaveRequest>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            Questionnaire? questionnaire = await _repository.GetAllDetailsAsync(id);
            if (questionnaire == null)
            {
                MessageHelper.NotFound("Questionnaire not found.");
            }
            return MessageHelper.Ok(_mapper.Map<QuestionnaireSaveRequest>(questionnaire),
                "Successfully retrieved questionnaires");
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(DataResult<QuestionnaireSaveRequest>))]
        [HttpPost]
        public async Task<IActionResult> SaveAsync([FromBody] QuestionnaireSaveRequest saveRequest)
        {
            if(ModelState.IsValid)
            {
                return await this.VerifyLogin( async loggedInUser =>
                {
                    Questionnaire newQuestionnaire = _mapper.Map<Questionnaire>(saveRequest);
                    if(!await _repository.InsertAsync(newQuestionnaire, loggedInUser.Username ?? "Admin"))
                    {
                        return MessageHelper.InternalServerError("Somehting went wrong in saving questionnaire");
                    }

                    return MessageHelper.Ok(_mapper.Map<QuestionnaireQuestionsResponseDto>(newQuestionnaire), "Successfully saved");
                });
            }
            return MessageHelper.BadRequest(ModelState);
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(DataResult<QuestionnaireSaveRequest>))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] QuestionnaireSaveRequest saveRequest)
        {
            if (ModelState.IsValid)
            {
                return await this.VerifyLogin(async loggedInUser =>
                {
                    Questionnaire? questionnaire = await _repository.GetAllDetailsAsync(id);
                    if (questionnaire == null)
                    {
                        MessageHelper.NotFound("Questionnaire not found.");
                    }
                    if(questionnaire!.Update(saveRequest,_mapper) && !await _repository.UpdateAsync(questionnaire!, loggedInUser.Username ?? "System"))
                    {
                        MessageHelper.InternalServerError("Something went wrong in updating questionnaire");
                    }

                    return MessageHelper.Ok(_mapper.Map<QuestionnaireSaveRequest>(questionnaire), "Successfully saved");
                });
            }
            return MessageHelper.BadRequest(ModelState);
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
        [HttpDelete("{id}")]
        public Task<IActionResult> Delete(int id)
        {
            return this.VerifyLogin(async loggedInUser =>
            {
                Questionnaire? questionnaire = id <= 0 ? null : await _repository.GetAllDetailsAsync(id);
                if (questionnaire == null)
                {
                    MessageHelper.NotFound("Questionnaire not found.");
                }
                if (!await _repository.DeleteAsync(id, loggedInUser.Username ?? "System"))
                {
                    MessageHelper.InternalServerError("Something went wrong in updating questionnaire");
                }

                return MessageHelper.Ok("Successfully deleted questionnaire");
            });
        }
    }
}

