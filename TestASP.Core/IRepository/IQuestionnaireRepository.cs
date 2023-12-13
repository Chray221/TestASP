using System;
using TestASP.Data;

namespace TestASP.Core.IRepository
{
	public interface IQuestionnaireRepository : IBaseRepository<Questionnaire>
    {
        Task<Questionnaire?> GetAllDetailsAsync(int id);
    }
}

