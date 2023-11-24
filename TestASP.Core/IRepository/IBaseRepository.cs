using System;
using TestASP.Data;

namespace TestASP.Core.IRepository
{
	public interface IBaseRepository<T> where T : BaseData
    {
        Task<bool> InsertAsync(T data);
        Task<bool> UpdateAsync(T data);
        Task<T?> GetAsync(int id);
        Task<List<T>> GetAsync(List<int> ids);
        Task<bool> DeleteAsync(int id);
    }
}

