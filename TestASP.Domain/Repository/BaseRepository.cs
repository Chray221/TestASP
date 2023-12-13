using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestASP.Core.IRepository;
using TestASP.Data;
using TestASP.Domain.Contexts;
using TestASP.Common.Extensions;
using System.Linq;

namespace TestASP.Domain.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseData
    {
        internal TestDbContext _dbContext;
        internal IQueryable<T> _entity;
        internal ILogger<BaseRepository<T>> _logger;

        private bool IsNoTracking;

        public BaseRepository(TestDbContext dbContext , ILogger<BaseRepository<T>> logger)
        {
            _dbContext = dbContext;
            _logger = logger;

            _entity = _dbContext.Set<T>().Where(e => !e.IsDeleted);
        }

        // Create
        virtual public async Task<bool> InsertAsync(T data, string createdBy = "System")
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
                data.CreatedBy = createdBy;
                await _dbContext.AddAsync(data);
                int insertedCount = await _dbContext.SaveChangesAsync();
                return insertedCount > 1;
            }
            return false;
        }


        // get 1
        virtual public async Task<T?> GetAsync(int id)
        {
            //if (id == Guid.Empty)
            if (id == default)
            {
                //throw new NullReferenceException($"Parammeter \"{typeof(T).Name}Id\" is empty in GetAsync");
                return null;
            }
            return await _dbContext.FindAsync<T>(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="pagination">page of pagination e.g 1 or 2 or 3</param>
        /// <param name="offset"> if pagiantion != null offset's value is 10 if null </param>
        /// <returns></returns>
        public Task<List<T>> GetAsync(List<int>? ids = null, int? pagination = null, int? offset = null)
        {
            return TryCatch(() =>
            {
                var query = _entity;
                if (ids != null && ids.Count > 0)
                {
                    query = _entity.Where(item => ids.Contains(item.Id));
                }

                if (pagination != null)
                {
                    offset = offset ?? 10;
                    query = query.Skip(pagination.Value * offset.Value)
                                 .Take(offset.Value);
                }
                return query.ToListAsync();
            });
        }

        //update
        virtual public async Task<bool> UpdateAsync(T data, string updatedBy = "System")
        {
            if (data == null)
            {
                throw new NullReferenceException($"Parammeter \"{typeof(T).Name}data is null in UpdateAsync");
            }

            if (data != null)
            {
                data.UpdatedAt = DateTime.Now;
                data.UpdatedBy = updatedBy;
                _dbContext.Update(data);
                int updatedCount = await _dbContext.SaveChangesAsync();
                return updatedCount > 1;
            }
            return false;
        }

        // delete
        virtual public async Task<bool> DeleteAsync(int id, string deletedBy = "System")
        {
            //if (id == Guid.Empty)
            if (id == default)
            {
                throw new NullReferenceException($"Parammeter \"{typeof(T).Name}Id\" is empty in DeleteAsync");
            }

            T? item = await _dbContext.FindAsync<T>(id);
            if (item != null)
            {
                item.IsDeleted = true;
                item.UpdatedAt = DateTime.Now;
                item.UpdatedBy = deletedBy;
                _dbContext.Update(item);
            }
            return item != null;
            
        }

        IQueryable<T>? _queryReference;
        internal IBaseRepository<T> IncludeReference(Func<IQueryable<T>, IQueryable<T>> action)
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

        public BaseRepository<T> AsNoTracking()
        {
            IsNoTracking = true;
            _entity = _entity.AsNoTracking();
            return this;
        }

        public BaseRepository<T> AsTracking()
        {
            _entity = _entity.AsTracking();
            IsNoTracking = false;
            return this;
        }

        internal IQueryable<T> GetEntity()
        {
            if(IsNoTracking)
            {
                //_entity = _entity.AsTracking();
                return _entity.AsNoTracking();
            }
            return _entity.AsTracking();
        }


    }
}
