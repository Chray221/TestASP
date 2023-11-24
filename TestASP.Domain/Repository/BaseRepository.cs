using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestASP.Core.IRepository;
using TestASP.Data;
using TestASP.Domain.Contexts;
using TestASP.Common.Extensions;

namespace TestASP.Domain.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseData
    {
        internal TestDbContext _dbContext;
        internal DbSet<T> _entity;
        internal ILogger<BaseRepository<T>> _logger;
        public BaseRepository(TestDbContext dbContext , ILogger<BaseRepository<T>> logger)
        {
            _dbContext = dbContext;
            _logger = logger;

            _entity = _dbContext.Set<T>();
        }

        virtual public async Task<bool> DeleteAsync(int id)
        {
            //if (id == Guid.Empty)
            if (id == default)
            {
                throw new NullReferenceException($"Parammeter \"{typeof(T).Name}Id\" is empty in DeleteAsync");
            }

            T deletedUser = await _dbContext.FindAsync<T>(id);
            if (deletedUser != null)
            {
                deletedUser.IsDeleted = true;
                deletedUser.UpdatedAt = DateTime.Now;
                _dbContext.Update(deletedUser);
            }
            return deletedUser != null;
            
        }

        virtual public async Task<T> GetAsync(int id)
        {
            //if (id == Guid.Empty)
            if (id == default)
            {
                throw new NullReferenceException($"Parammeter \"{typeof(T).Name}Id\" is empty in GetAsync");
            }
            return await _dbContext.FindAsync<T>(id);
        }

        public Task<List<T>> GetAsync(List<int> ids)
        {
            return _entity.Where(item => ids.Contains(item.Id)).ToListAsync();
        }

        virtual public async Task<bool> InsertAsync(T data)
        {
            if (data == null)
            {
                throw new NullReferenceException($"Parammeter \"{typeof(T).Name} data\" is null in InsertAsync");
            }

            if (data != null)
            {
                //if(data.Id == Guid.Empty)
                //{
                //    data.Id = Guid.NewGuid();
                //}
                data.CreatedAt = DateTime.Now;
                await _dbContext.AddAsync(data);
                await _dbContext.SaveChangesAsync();
                return true;
            }            
            return false;
        }

        virtual public async Task<bool> UpdateAsync(T data)
        {
            if (data == null)
            {
                throw new NullReferenceException($"Parammeter \"{typeof(T).Name}data is null in UpdateAsync");
            }

            if (data != null)
            {
                data.IsDeleted = true;
                data.UpdatedAt = DateTime.Now;
                _dbContext.Update(data);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        IQueryable<T>? _queryReference;
        internal BaseRepository<T> IncludeReference(Func<IQueryable<T>, IQueryable<T>> action)
        {
            return this;
        }


        internal TResult TryCatch<TResult>([NotNull]Func<TResult> action)
        {
            int count = 0;

            SQL_REQUEST:
            try
            {
                return action.Invoke();
            }
            catch(Exception ex)
            {
                _logger.LogException(ex);
                if(count < 3)
                {
                    count++;
                    goto SQL_REQUEST;
                }
            }
            return default;
        }


    }
}
