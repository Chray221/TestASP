using System;
using TestASP.Web.Models;
using TestASP.Model;

namespace TestASP.Web.IServices
{
	public interface IAuthService
	{
        Task<ApiResult<UserDto>> LoginAsync(SignInUserRequestDto singin);
        Task<ApiResult<UserDto>> SignupAsync(UserSignupRequest signup);
    }
}

