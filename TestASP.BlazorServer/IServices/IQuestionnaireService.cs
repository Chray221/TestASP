using System;
using TestASP.BlazorServer.Models;
using TestASP.Model;
using TestASP.Model.Questionnaires;

namespace TestASP.BlazorServer.IServices
{
	public interface IQuestionnaireService
    {
        Task<ApiResult<List<QuestionnaireResponseDto>>> GetAsync();
        Task<ApiResult<QuestionnaireResponseDto>> GetAsync(int questionId);
        Task<ApiResult<QuestionnaireQuestionsResponseDto>> GetWithQuestionAnswerAsync(int questionId);
        Task<ApiResult<QuestionnaireResponseDto>> SaveAsync(int questionId, List<QuestionnaireAnswerSubAnswerRequestDto> answers);
    }
}

