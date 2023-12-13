using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    //[Route("v{version:apiversion}/api/User/{userId}/Questionnaire")]
    [Route("v{version:apiversion}/api/Questionnaire")]
    public class UserQuestionnaireController
        : MyRepositoryControllerBase<IQuestionnaireRepository, Questionnaire>
    {
        public UserQuestionnaireController(
            IWebHostEnvironment environment,
            IQuestionnaireRepository repository,
            IMapper mapper) : base(environment, repository, mapper)
        {
        }

        //[Authorize("user")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(DataResult<List<QuestionnaireResponseDto>>))]
        [HttpGet("Answer")]
        public Task<IActionResult> GetAsync()
        {
            return this.VerifyLogin(async loggedInUser =>
            {
                List<Questionnaire> questionnaires = await _repository.GetAsync();
                // get user answered questionnaire
                if (questionnaires == null)
                {
                    MessageHelper.InternalServerError("Something went wrong in retrieving questionnaires.");
                }
                return MessageHelper.Ok(questionnaires!.Select(_mapper.Map<QuestionnaireQuestionsResponseDto>),
                                         "Successfully retrieved questionnaires");
            });
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(DataResult<QuestionnaireQuestionsResponseDto>))]
        [HttpGet("{id}/Answer")]
        public Task<IActionResult> GetAsync(int id)
        {
            return this.VerifyLogin(async loggedInUser =>
            {
                Questionnaire? questionnaire = await _repository.GetAllDetailsAsync(id);
                if (questionnaire == null)
                {
                    MessageHelper.NotFound("Questionnaire not found.");
                }
                return MessageHelper.Ok(_mapper.Map<QuestionnaireQuestionsResponseDto>(questionnaire),
                                        "Successfully retrieved questionnaires");
            });
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(DataResult<QuestionnaireQuestionsResponseDto>))]
        [HttpPost("{id}/Answer")]
        public async Task<IActionResult> SaveAsync(int id, [FromBody] QuestionnaireSaveRequest saveRequest)
        {
            if (ModelState.IsValid)
            {
                return await this.VerifyLogin(async loggedInUser =>
                {
                    Questionnaire? questionnaire = await _repository.GetAsync(id);
                    if(questionnaire == null)
                    {
                        return MessageHelper.BadRequest("Questionnaire not found");
                    }

                    return MessageHelper.Ok(_mapper.Map<QuestionnaireQuestionsResponseDto>(questionnaire), "Successfully saved");
                });
            }
            return MessageHelper.BadRequest(ModelState);
        }

        [HttpPut("{id}/Answer")]
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
                    if (questionnaire!.Update(saveRequest, _mapper) && !await _repository.UpdateAsync(questionnaire!, loggedInUser.Username ?? "System"))
                    {
                        MessageHelper.InternalServerError("Something went wrong in updating questionnaire");
                    }

                    return MessageHelper.Ok(_mapper.Map<QuestionnaireQuestionsResponseDto>(questionnaire), "Successfully saved");
                });
            }
            return MessageHelper.BadRequest(ModelState);
        }

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

