using System;
using TestASP.Data;

namespace TestASP.Core.IRepository
{
	public interface IBaseRepository<T> where T : BaseData
    {
        Task<bool> InsertAsync(T data, string createdBy = "System");
        Task<bool> UpdateAsync(T data, string updatedBy = "System");
        Task<T?> GetAsync(int id);
        Task<List<T>> GetAsync(List<int>? ids = null, int? pagination = null, int? offset = null);
        Task<bool> DeleteAsync(int id, string deletedBy = "System");
    }
}

