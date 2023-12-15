using System;
using TestASP.BlazorServer.Models;
using TestASP.Model;
using TestASP.Model.Questionnaires;

namespace TestASP.BlazorServer.IServices
{
	public interface IQuestionnaireService
    {
        Task<ApiResult<List<QuestionnaireResponseDto>>> GetAdminAsync();
        Task<ApiResult<List<UserQuestionnaireResponseDto>>> GetAsync(int userId);
        Task<ApiResult<QuestionnaireQuestionsResponseDto>> GetAsync(int userId, int questionnaireId, int? userQuestionnaireId = null);
        Task<ApiResult<QuestionnaireQuestionsResponseDto>> GetWithQuestionAnswerAsync(int userId, int questionnaireId, int? userQuestionnaireId = null);
        Task<ApiResult<QuestionnaireQuestionsResponseDto>> SaveAsync(int userId, int questionnaireId, List<QuestionnaireAnswerSubAnswerRequestDto> answers);
        Task<ApiResult<QuestionnaireQuestionsResponseDto>> UpdateAsync(int userId, int questionnaireId, int userQuestionnaireId, List<QuestionnaireAnswerSubAnswerRequestDto> answers);

        //Task<ApiResult<QuestionnaireQuestionsResponseDto>> GetWithQuestionAnswerAsync(int questionId);
        //Task<ApiResult<QuestionnaireResponseDto>> SaveAsync(int questionId, List<QuestionnaireAnswerSubAnswerRequestDto> answers);
    }
}

