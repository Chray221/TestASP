using TestASP.Web.IServices;
using TestASP.Web.Models;
using TestASP.Common.Utilities;
using TestASP.Model;

namespace TestASP.Web.Services
{
    public class AuthService : BaseApiService, IAuthService
	{
        public AuthService(IHttpClientFactory httpClient, ILogger<AuthService> logger, ConfigurationManager configuration) : base(httpClient, logger, configuration)
        {
        }

        public Task<ApiResult<UserDto>> LoginAsync(SignInUserRequestDto singin)
        {
            return SendAsync<SignInUserRequestDto, UserDto>(ApiRequest.PostRequest(ApiEndpoints.LoginAuthUrl, singin));
        }

        public Task<ApiResult<UserDto>> SignupAsync(UserSignupRequest signup)
        {
            return SendAsync<UserSignupRequest, UserDto>(ApiRequest.PostRequest(ApiEndpoints.SignupAuthUrl, signup, true));
        }
    }
}

