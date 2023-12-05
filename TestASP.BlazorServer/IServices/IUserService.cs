using System;
using TestASP.BlazorServer.Models;
using TestASP.Model;

namespace TestASP.BlazorServer.IServices
{
	public interface IUserService
    {
        Task<ApiResult<UserDto>> GetAsync(int id);
        Task<ApiResult<UserDto>> GetAsync(string username);
    }
}

