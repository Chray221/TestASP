using System;
using TestASP.BlazorServer.Models;
using TestASP.Model;

namespace TestASP.BlazorServer.IServices
{
	public interface IAuthService
	{
        Task<ApiResult<UserDto>> LoginAsync(SignInUserRequestDto singin);
        Task<ApiResult<UserDto>> SignupAsync(UserSignupRequest signup);
    }
}

