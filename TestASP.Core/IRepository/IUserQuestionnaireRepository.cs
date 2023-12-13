using System;
using TestASP.Data.Questionnaires;

namespace TestASP.Core.IRepository
{
	public interface IUserQuestionnaireRepository : IBaseRepository<UserQuestionnaire>
	{
        Task<List<UserQuestionnaire>> GetByUserIdAsync(int userId);
        Task<UserQuestionnaire?> GetAllDetailAsync(int id);
        
    }
}

