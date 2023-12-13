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

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(DataResult<List<QuestionnaireResponseDto>>))]
        [HttpGet]
        public Task<IActionResult> GetAsync()
        {
            return this.VerifyLogin( async loggedInUser =>
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
                            if(userQuestionnaireDto.IsAnswered)
                            {
                                UserQuestionnaireResponseDto newDto = _mapper.Map<UserQuestionnaireResponseDto>(userQuestionnaireDto);
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
        [HttpGet("{id}/Answer/{userQuestionnaireId}")]
        public Task<IActionResult> GetAsync(int id, int? userQuestionnaireId)
        {
            return this.VerifyLogin(async loggedInUser =>
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
            int id,
            [FromBody] List<QuestionnaireAnswerSubAnswerRequestDto> saveRequest,
            ValidationService validationService)
        {
            Questionnaire questionnaire = await validationService.ValidateSaveQuestionnaireAsync(ModelState, id, saveRequest);

            if (ModelState.IsValid)
            {
                return await this.VerifyLogin(async loggedInUser =>
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

                    return MessageHelper.Ok(_mapper.Map<QuestionnaireQuestionsResponseDto>(questionnaire), "Successfully saved");
                });
            }
            return MessageHelper.BadRequest(ModelState);
        }

        [HttpPut("{id}/Answer/{userQuestionnaireId}")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(DataResult<QuestionnaireQuestionsResponseDto>))]
        public async Task<IActionResult> UpdateAsync(int id, int userQuestionnaireId, [FromBody] QuestionnaireAnswerSubAnswerRequestDto updateRequest)
        {
            if (ModelState.IsValid)
            {
                //return await this.VerifyLogin(async loggedInUser =>
                //{
                //    Questionnaire? questionnaire = await _repository.GetAllDetailsAsync(id);
                //    if (questionnaire == null)
                //    {
                //        MessageHelper.NotFound("Questionnaire not found.");
                //    }
                //    if (questionnaire!.Update(updateRequest, _mapper) && !await _repository.UpdateAsync(questionnaire!, loggedInUser.Username ?? "System"))
                //    {
                //        MessageHelper.InternalServerError("Something went wrong in updating questionnaire");
                //    }

                //    return MessageHelper.Ok(_mapper.Map<QuestionnaireQuestionsResponseDto>(questionnaire), "Successfully saved");
                //});
            }
            return MessageHelper.BadRequest(ModelState);
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
        [HttpDelete("{id}/Answer/{userQuestionnaireId}")]
        public Task<IActionResult> Delete(int id, int userQuestionnaireId,
            IDataValidationService dataValidationService)
        {
            return this.VerifyLogin(async loggedInUser =>
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

