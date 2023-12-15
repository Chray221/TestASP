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
using TestASP.API.Services;
using TestASP.Common.Utilities;
using TestASP.Core.IRepository;
using TestASP.Core.IService;
using TestASP.Data;
using TestASP.Data.Questionnaires;
using TestASP.Model;
using TestASP.Model.Questionnaires;
using TestASP.Model.Request.Questionnaires;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestASP.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiversion}/api/User/{userId}/Questionnaire")]
    //[Route("v{version:apiversion}/api/Questionnaire")]
    public class UserQuestionnaireController
        : MyRepositoryControllerBase<IUserQuestionnaireRepository, UserQuestionnaire>
    {
        private readonly IQuestionnaireRepository _questionnaireRepository;

        public UserQuestionnaireController(
            IWebHostEnvironment environment,
            IUserQuestionnaireRepository repository,
            IQuestionnaireRepository questionnaireRepository,
            IMapper mapper) : base(environment, repository, mapper)
        {
            _questionnaireRepository = questionnaireRepository;
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(DataResult<List<UserQuestionnaireResponseDto>>))]
        [HttpGet]
        public Task<IActionResult> GetAsync()
        {
            return VerifyLogin( async loggedInUser =>
            {
                List<Questionnaire> questionnaires = await _questionnaireRepository.GetAsync();
                List<UserQuestionnaire> userQuestionnaires = await _repository.GetByUserIdAsync(loggedInUser.Id);
                if (questionnaires == null)
                {
                    MessageHelper.InternalServerError("Something went wrong in retrieving questionnaires.");
                }
                List<UserQuestionnaireResponseDto> userQuestionnaireDtos = questionnaires!.SelectMapList<UserQuestionnaireResponseDto>(_mapper);
                if (userQuestionnaires?.Count > 0)
                {
                    foreach(var userQuestionnaire in userQuestionnaires)
                    {
                        UserQuestionnaireResponseDto? userQuestionnaireDto = userQuestionnaireDtos.FirstOrDefault(dto => dto.Id == userQuestionnaire.QuestionnaireId);
                        if(userQuestionnaireDto != null)
                        {
                            userQuestionnaireDto.DateAnswered = userQuestionnaire.UpdatedAt ?? userQuestionnaire.CreatedAt;
                            if (userQuestionnaireDto.IsAnswered)
                            {
                                UserQuestionnaireResponseDto newDto = userQuestionnaireDto.Clone();
                                newDto.UserQuestionnaireId = userQuestionnaire.Id;
                                userQuestionnaireDtos.Add(newDto);
                            }
                            else
                            {
                                userQuestionnaireDto.UserQuestionnaireId = userQuestionnaire.Id;
                            }
                        }
                    }
                }
                return MessageHelper.Ok(userQuestionnaireDtos, "Successfully retrieved user questionnaires");
            });
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(DataResult<QuestionnaireQuestionsResponseDto>))]
        [HttpGet("{id}/Answer/{userQuestionnaireId}" , Name = "GetUserQuestionnaireAnswer")]
        [HttpGet("{id}/Answer")]
        [ActionName("GetUserQuestionnaireAnswer")]
        public Task<IActionResult> GetAsync([FromRoute]int id, [FromRoute]int? userQuestionnaireId)
        {
            return VerifyLogin(async loggedInUser =>
            {
                if (userQuestionnaireId == null)
                {
                    Questionnaire? questionnaire = await _questionnaireRepository.GetAllDetailsAsync(id);
                    if (questionnaire == null)
                    {
                        MessageHelper.BadRequest("Questionnaire not found.");
                    }
                    return MessageHelper.Ok(_mapper.Map<QuestionnaireQuestionsResponseDto>(questionnaire),
                                            "Successfully retrieved questionnaire");
                }

                UserQuestionnaire? userQuestionnaire = await _repository.GetAllDetailAsync(userQuestionnaireId ?? 0);
                if (userQuestionnaire == null)
                {
                    MessageHelper.BadRequest("Questionnaire answer not found.");
                }
                return MessageHelper.Ok(_mapper.Map<QuestionnaireQuestionsResponseDto>(userQuestionnaire),
                                        "Successfully retrieved questionnaire");
            });
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(DataResult<QuestionnaireQuestionsResponseDto>))]
        [HttpPost("{id}/Answer")]
        public async Task<IActionResult> SaveAsync(
            [FromRoute] int id,
            [FromBody] List<QuestionnaireAnswerSubAnswerRequestDto> saveRequest,
            [FromServices] ValidationService validationService)
        {
            Questionnaire questionnaire = await validationService.ValidateSaveQuestionnaireAsync(ModelState, id, saveRequest);

            return await VerifyLogin(async loggedInUser =>
            {
                UserQuestionnaire newUserQuestionnaire = new UserQuestionnaire(loggedInUser.Id, id);
                newUserQuestionnaire.QuestionAnswers = new List<QuestionnaireAnswer>();
                foreach (var answer in saveRequest)
                {
                    newUserQuestionnaire.QuestionAnswers.Add(_mapper.Map<QuestionnaireAnswer>(answer));
                }
                if (!await _repository.InsertAsync(newUserQuestionnaire, loggedInUser.Username ?? "System"))
                {
                    return MessageHelper.InternalServerError("Something went wrong in saving questionnaire answer");
                }

                return MessageHelper.Ok(_mapper.Map<QuestionnaireQuestionsResponseDto>(newUserQuestionnaire), "Successfully saved");
                //return RedirectToAction
                //return RedirectToAction("GetUserQuestionnaireAnswer",new { id = newUserQuestionnaire.Id });
            });
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(DataResult<QuestionnaireQuestionsResponseDto>))]
        [HttpPut("{id}/Answer/{userQuestionnaireId}")]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute] int id,
            [FromRoute] int userQuestionnaireId,
            [FromBody] List<QuestionnaireAnswerSubAnswerRequestDto> updateRequest,
            [FromServices] ValidationService validationService)
        {
            Questionnaire questionnaire = await validationService.ValidateSaveQuestionnaireAsync(ModelState, id, updateRequest);

            return await VerifyLogin(async loggedInUser =>
            {
                UserQuestionnaire? userQuestionnaire = await _repository.GetAllDetailAsync(userQuestionnaireId);
                if(userQuestionnaire == null)
                {
                    return MessageHelper.BadRequest("User questionnaire answer not found.");
                }

                if (userQuestionnaire!.Update(updateRequest, _mapper) && !await _repository.InsertAsync(userQuestionnaire, loggedInUser.Username ?? "System"))
                {
                    return MessageHelper.InternalServerError("Something went wrong in saving questionnaire answer");
                }

                return MessageHelper.Ok(_mapper.Map<QuestionnaireQuestionsResponseDto>(userQuestionnaire), "Successfully updated");
                //return RedirectToAction("GetUserQuestionnaireAnswer", new { id = userQuestionnaire.Id });
            });
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
        [HttpDelete("{id}/Answer/{userQuestionnaireId}")]
        public Task<IActionResult> DeleteAsync(
            [FromRoute] int id,
            [FromRoute] int userQuestionnaireId,
            IDataValidationService dataValidationService)
        {
            return VerifyLogin(async loggedInUser =>
            {
                if (!await dataValidationService.IsDataExist<Questionnaire>(id))
                {
                    MessageHelper.NotFound("Questionnaire not found.");
                }
                if (!await dataValidationService.IsDataExist<UserQuestionnaire>(userQuestionnaireId))
                {
                    MessageHelper.NotFound("User questionnaire answer not found.");
                }
                if (!await _repository.DeleteAsync(userQuestionnaireId, loggedInUser.Username ?? "System"))
                {
                    MessageHelper.InternalServerError("Something went wrong in updating questionnaire");
                }

                return MessageHelper.Ok("Successfully deleted questionnaire");
            });
        }
    }
}

