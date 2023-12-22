using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using TestASP.Web.IServices;
using TestASP.Web.Models;
using TestASP.Common.Utilities;
using TestASP.Model;

namespace TestASP.Web.Services
{
    public class UserService : BaseApiService, IUserService
	{

        // public UserService(
        //     IHttpClientFactory httpClient,
        //     ILogger<UserService> logger,
        //     ConfigurationManager configuration,
        //     ProtectedLocalStorage localStorage)
        //     : base(httpClient, logger, configuration, localStorage)
        // {
        // }
        public UserService(
            IHttpClientFactory httpClient,
            ILogger<UserService> logger,
            ConfigurationManager configuration,
            IHttpContextAccessor httpContext)
             : base(httpClient, logger, configuration, httpContext)
        {
        }

        public Task<ApiResult<UserDto>> GetAsync(int id)
        {
            return SendAsync<UserDto>(ApiRequest.GetRequest($"{ApiEndpoints.UserUrl}/{id}"));
        }

        public Task<ApiResult<UserDto>> GetAsync(string username)
        {
            return SendAsync<UserDto>(ApiRequest.GetRequest($"{ApiEndpoints.UserUrl}/{username}"));
        }
    }
}

