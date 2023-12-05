using System;
using TestASP.BlazorServer.Models;
using TestASP.Model;

namespace TestASP.BlazorServer.IServices
{
	public interface ICRUDBaseApiService<T> where T: BaseDto
    {
        Task<ApiResult<List<T>>> GetAllAsync();
        Task<ApiResult<T>> GetAsync(int id);
        Task<ApiResult<T>> CreateAsync(T newData);
        Task<ApiResult<T>> UpdateAsync(T updateData);
        Task<ApiResult<T>> DeleteAsync(int id);
    }

    public interface ICRUDBaseApiService
    {
        Task<ApiResult<List<T>>> GetAllAsync<T>() where T : BaseDto;
        Task<ApiResult<T>> GetAsync<T>(int id) where T : BaseDto;
        Task<ApiResult<T>> CreateAsync<T>(T newData) where T : BaseDto;
        Task<ApiResult<T>> UpdateAsync<T>(T updateData) where T : BaseDto;
        Task<ApiResult<T>> DeleteAsync<T>(int id) where T : BaseDto;
    }
}

