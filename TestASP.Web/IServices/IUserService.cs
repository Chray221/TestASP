using TestASP.Web.Models;
using TestASP.Model;

namespace TestASP.Web.IServices
{
	public interface IUserService
    {
        Task<ApiResult<UserDto>> GetAsync(int id);
        Task<ApiResult<UserDto>> GetAsync(string username);
    }
}

