using System;
using Microsoft.EntityFrameworkCore;
using TestASP.Core.IService;
using TestASP.Data;
using TestASP.Domain.Contexts;

namespace TestASP.Domain.Services
{
	public class DataValidationService : IDataValidationService
	{
        private readonly TestDbContext _database;

        public DataValidationService(TestDbContext dbContext)
        {
            _database = dbContext;
        }

        public Task<bool> IsDataExist<T>(int id)
            where T : BaseData
        {
            DbSet<T> entity = _database.Set<T>();
            return entity.AnyAsync(e => !e.IsDeleted && e.Id == id);
        }

    }
}

