using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using TestASP.BlazorServer.IServices;
using TestASP.BlazorServer.Models;
using TestASP.Common.Utilities;
using TestASP.Model.Questionnaires;
using TestASP.Data.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestASP.BlazorServer.Services
{
    public class QuestionnaireService : BaseApiService, IQuestionnaireService
    {
        public QuestionnaireService(
            IHttpClientFactory httpClient,
            ILogger<QuestionnaireService> logger,
            ConfigurationManager configuration,
            ProtectedLocalStorage localStorage)
            : base(httpClient, logger, configuration, localStorage)
        {
        }

        public Task<ApiResult<List<QuestionnaireResponseDto>>> GetAsync()
        {
            //return SendAsync<List<QuestionnaireResponseDto>>(ApiRequest.GetRequest($"{ApiEndpoints.Questionnaire}"));
            return Task.FromResult(ApiResult.Success(new List<QuestionnaireResponseDto>()
            {
                new QuestionnaireResponseDto()
                {
                    Id = 1,
                    Name = "Why Questionnaire",
                    Description = "Why is this questionnaire asking why?",
                }
            }));
        }

        public Task<ApiResult<QuestionnaireResponseDto>> GetAsync(int questionId)
        {
            //return SendAsync<QuestionnaireResponseDto>(ApiRequest.GetRequest($"{ApiEndpoints.Questionnaire}/{questionId}"));
            return Task.FromResult(ApiResult.Success(
                new QuestionnaireResponseDto() {
                    Id = 1,
                    Name = "Why Questionnaire",
                    Description = "Why is this questionnaire asking why?",                    
                }));
        }

        public Task<ApiResult<QuestionnaireQuestionsResponseDto>> GetWithQuestionAnswerAsync(int questionId)
        {
            //return SendAsync<QuestionnaireResponseDto>(ApiRequest.GetRequest($"{ApiEndpoints.LoginAuthUrl}/{questionId}"));
            return Task.FromResult(ApiResult.Success(
                new QuestionnaireQuestionsResponseDto()
                {
                    Id = questionId,
                    Name = "Why Questionnaire",
                    Description = "Why is this questionnaire asking why?",
                    QuestionAnswers = new List<QuestionAnswerSubQuestionAnswerResponseDto>()
                    {
                        new QuestionAnswerSubQuestionAnswerResponseDto()
                        {
                            QuestionId = 1,
                            Number = "1",
                            Question = "Is this question asking why?",
                            QuestionTypeId = QuestionTypeEnum.Question,
                            AnswerTypeId = AnswerTypeEnum.BooleanWithSubQuestion,
                            SubQuestionAnswers = new List<SubQuestionAnswerResponseDto>()
                            {
                                new SubQuestionAnswerResponseDto()
                                {
                                    SubQuestionId = 1,
                                    Number = "1.a",
                                    Question = "Do you know the reason?",
                                    QuestionTypeId = QuestionTypeEnum.BooleanYesSubQuestion,
                                    AnswerTypeId = AnswerTypeEnum.Boolean,
                                },
                                new SubQuestionAnswerResponseDto()
                                {
                                    SubQuestionId = 2,
                                    Number = "1.b",
                                    Question = "What do you this is the reason?",
                                    QuestionTypeId = QuestionTypeEnum.BooleanYesSubQuestion,
                                    AnswerTypeId = AnswerTypeEnum.Text,
                                },
                                new SubQuestionAnswerResponseDto()
                                {
                                    SubQuestionId = 3,
                                    Number = "1.c",
                                    Question = "When did you answer this question?",
                                    QuestionTypeId = QuestionTypeEnum.BooleanYesSubQuestion,
                                    AnswerTypeId = AnswerTypeEnum.Date,
                                },
                                new SubQuestionAnswerResponseDto()
                                {
                                    SubQuestionId = 4,
                                    Number = "1.d",
                                    Question = "How may sub question is this question?",
                                    QuestionTypeId = QuestionTypeEnum.BooleanYesSubQuestion,
                                    AnswerTypeId = AnswerTypeEnum.Number,
                                },
                                new SubQuestionAnswerResponseDto()
                                {
                                    SubQuestionId = 5,
                                    Number = "-",
                                    Question = "How you know?",
                                    QuestionTypeId = QuestionTypeEnum.BooleanNoSubQuestion,
                                    AnswerTypeId = AnswerTypeEnum.Text,
                                },
                                new SubQuestionAnswerResponseDto()
                                {
                                    SubQuestionId = 6,
                                    Number = "1.e",
                                    Question = "Do you want to know why this questionnaire is asking why",
                                    QuestionTypeId = QuestionTypeEnum.SubQuestion,
                                    AnswerTypeId = AnswerTypeEnum.Boolean,
                                },
                            },
                        },
                        new QuestionAnswerSubQuestionAnswerResponseDto()
                        {
                            QuestionId = 2,
                            Number = "2",
                            Question = "Do you this is question has sub question?",
                            QuestionTypeId = QuestionTypeEnum.Question,
                            AnswerTypeId = AnswerTypeEnum.Boolean
                        },
                        new QuestionAnswerSubQuestionAnswerResponseDto()
                        {
                            QuestionId = 3,
                            Number = "3",
                            Question = "How about this?",
                            QuestionTypeId = QuestionTypeEnum.Question,
                            AnswerTypeId = AnswerTypeEnum.Boolean,
                            SubQuestionAnswers = new List<SubQuestionAnswerResponseDto>()
                            {
                                new SubQuestionAnswerResponseDto()
                                {
                                    SubQuestionId = 7,
                                    Number = "3.a",
                                    Question = "How may sub question is this question?",
                                    QuestionTypeId = QuestionTypeEnum.BooleanYesSubQuestion,
                                    AnswerTypeId = AnswerTypeEnum.Number,
                                },
                            }
                        },
                        new QuestionAnswerSubQuestionAnswerResponseDto()
                        {
                            QuestionId = 4,
                            Number = "4.",
                            Question = "How many did you answer in this questionnaire?",
                            QuestionTypeId = QuestionTypeEnum.Question,
                            AnswerTypeId = AnswerTypeEnum.Number
                        },
                        new QuestionAnswerSubQuestionAnswerResponseDto()
                        {
                            QuestionId = 5,
                            Number = "5.",
                            Question = "What do you think of this questionnaire?(explain why)",
                            QuestionTypeId = QuestionTypeEnum.Question,
                            AnswerTypeId = AnswerTypeEnum.Text
                        },
                        new QuestionAnswerSubQuestionAnswerResponseDto()
                        {
                            QuestionId = 6,
                            Number = "5.",
                            Question = "How do you feel about this questionnaire?",
                            QuestionTypeId = QuestionTypeEnum.Question,
                            AnswerTypeId = AnswerTypeEnum.MultipleChoice,
                            Choices = new List<QuestionChoiceDto>()
                            {
                                new QuestionChoiceDto()
                                {
                                    Id = 1,
                                    Name = "Very Unsatisfied",
                                    Value = "0"
                                },
                                new QuestionChoiceDto()
                                {
                                    Id = 2,
                                    Name = "Unsatisfied",
                                    Value = "1"
                                },
                                new QuestionChoiceDto()
                                {
                                    Id = 3,
                                    Name = "Slightly Satisfied",
                                    Value = "2"
                                },
                                new QuestionChoiceDto()
                                {
                                    Id = 4,
                                    Name = "Satisfied",
                                    Value = "3"
                                },
                                new QuestionChoiceDto()
                                {
                                    Id = 5,
                                    Name = "Very Satisfied",
                                    Value = "4"
                                }
                            }
                        },
                    }
                }));
        }

        public Task<ApiResult<QuestionnaireResponseDto>> SaveAsync(int questionId, List<QuestionnaireAnswerSubAnswerRequestDto> answers)
        {
            return SendAsync<List<QuestionnaireAnswerSubAnswerRequestDto>, QuestionnaireResponseDto>(
                ApiRequest.PostRequest(
                    ApiEndpoints.FromFormat(ApiEndpoints.SaveQuestionnaire, questionId),
                    answers));
        }
    }
}

