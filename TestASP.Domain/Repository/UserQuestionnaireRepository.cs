using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestASP.Core.IRepository;
using TestASP.Data;
using TestASP.Data.Questionnaires;
using TestASP.Domain.Contexts;

namespace TestASP.Domain.Repository
{
    public class UserQuestionnaireRepository : BaseRepository<UserQuestionnaire>, IUserQuestionnaireRepository
    {
        public UserQuestionnaireRepository(
            TestDbContext dbContext,
            ILogger<UserQuestionnaireRepository> logger)
            : base(dbContext, logger)
        {
        }

        public Task<UserQuestionnaire?> GetAllDetailAsync(int id)
        {
            return TryCatch(() => _entity.Include(uQstnr => uQstnr.Questionnaire)
                                         .ThenInclude(questionnaire => questionnaire!.Questions.Where(item => !item.IsDeleted))
                                         .ThenInclude(question => question.SubQuestions!.Where(item => !item.IsDeleted))
                                         .ThenInclude(subQuestion => subQuestion.Choices!.Where(item => !item.IsDeleted))
                                         .Include(uQstnr => uQstnr.Questionnaire)
                                         .ThenInclude(questionnaire => questionnaire!.Questions.Where(item => !item.IsDeleted))
                                         .ThenInclude(question => question.Choices!.Where(item => !item.IsDeleted))
                                         .Include( uQstnr => uQstnr.QuestionAnswers!.Where(item => !item.IsDeleted))
                                         .ThenInclude(answer => answer.SubAnswers!.Where( item => !item.IsDeleted))
                                         .FirstOrDefaultAsync(uQuestionnaire => uQuestionnaire.Id == id));
        }

        public Task<List<UserQuestionnaire>> GetByUserIdAsync(int userId)
        {
            return TryCatch(() => _entity.Where(uQuestionnaire => uQuestionnaire.UserId == userId).ToListAsync());
        }
    }
}

