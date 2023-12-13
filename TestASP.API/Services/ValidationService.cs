using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TestASP.API.Configurations.Filters;
using TestASP.API.Helpers;
using TestASP.Core.IRepository;
using TestASP.Core.IService;
using TestASP.Data;
using TestASP.Model.Questionnaires;

namespace TestASP.API.Services
{
	public class ValidationService
    {
        public IDataValidationService _dataValidationService { get; }
        public IQuestionnaireRepository _questionnaireRepository { get; }

        public ValidationService(
			IDataValidationService dataValidationService,
			IQuestionnaireRepository questionnaireRepository)
		{
            _dataValidationService = dataValidationService;
            _questionnaireRepository = questionnaireRepository;
        }

        public async Task<Questionnaire> ValidateSaveQuestionnaireAsync(ModelStateDictionary ModelState, int questionnaireId, List<QuestionnaireAnswerSubAnswerRequestDto> saveRequest)
        {
            #region Validations
            int count = 0;
            int subCount = 0;
            string prop = "";
            string subProp = "";
            foreach (var request in saveRequest)
            {
                prop = $"[{count++}]";
                if (!await _dataValidationService.IsDataExist<QuestionnaireQuestion>(request.QuestionId))
                {
                    ModelState.AddModelError($"{prop}.{nameof(request.QuestionId)}", "QuestionId not found.");
                }
                if (request.AnswerId != null && !await _dataValidationService.IsDataExist<QuestionnaireQuestionChoice>(request.AnswerId ?? 0))
                {
                    ModelState.AddModelError($"{prop}.{nameof(request.AnswerId)}", "AnswerId not found.");
                }
                subCount = 0;
                foreach (var subAnswer in request.SubAnswers)
                {
                    subProp = $"{prop}.{nameof(request.SubAnswers)}[{subCount++}]";
                    if (!await _dataValidationService.IsDataExist<QuestionnaireQuestion>(subAnswer.SubQuestionId))
                    {
                        ModelState.AddModelError($"{subProp}.{nameof(subAnswer.SubQuestionId)}", "SubQuestionId not found.");
                    }
                    if (subAnswer.AnswerId != null && !await _dataValidationService.IsDataExist<QuestionnaireQuestionChoice>(subAnswer.AnswerId ?? 0))
                    {
                        ModelState.AddModelError($"{subProp}.{nameof(subAnswer.AnswerId)}", "AnswerId not found.");
                    }
                }
            }
            #endregion
            Questionnaire? questionnaire = await _questionnaireRepository.GetAllDetailsAsync(questionnaireId);
            if(questionnaire == null)
            {
                throw ActionResultException.BadRequest("Questionnaire not found");
            }                                            
            foreach (var question in questionnaire.Questions)
            {
                var answer = saveRequest.FirstOrDefault(answer => answer.QuestionId == question.Id);
                if(answer == null)
                {
                    throw ActionResultException.BadRequest($"Questionnaire question {question.Number} not answered");
                }
                if (!string.IsNullOrEmpty(answer.Answer))
                {
                    foreach (var subQuestion in question.GetSubQuestionsFromAnswer(answer.Answer) ?? new List<QuestionnaireSubQuestion>())
                    {
                        var subAnswer = answer.SubAnswers.FirstOrDefault(sub => sub.SubQuestionId == subQuestion.Id);
                        if(subAnswer == null)
                        {
                            throw ActionResultException.BadRequest($"Questionnaire question {question.Number} with sub question {subQuestion.Number} not answered");
                        }
                    }
                }
                
            }

            return questionnaire;
        }

    }
}

