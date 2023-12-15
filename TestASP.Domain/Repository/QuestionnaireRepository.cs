﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestASP.Core.IRepository;
using TestASP.Data;
using TestASP.Domain.Contexts;

namespace TestASP.Domain.Repository
{
	public class QuestionnaireRepository : BaseRepository<Questionnaire>, IQuestionnaireRepository
	{

        public QuestionnaireRepository(TestDbContext dbContext,
            ILogger<QuestionnaireRepository> logger) : base(dbContext, logger)
        {
        }

        public Task<Questionnaire?> GetAllDetailsAsync(int id)
        {
            return TryCatch(() => _entity.Include( questionnaire => questionnaire.Questions)
                                         .ThenInclude(question => question.SubQuestions!)
                                         .ThenInclude(subQuestion => subQuestion.Choices)
                                         .Include(questionnaire => questionnaire.Questions)
                                         .ThenInclude(question => question.Choices)
                                         .FirstOrDefaultAsync( questionnaire => questionnaire.Id == id) );
        }
    }
}
