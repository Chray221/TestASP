using System;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using TestASP.BlazorServer.IServices;
using TestASP.BlazorServer.Models;
using TestASP.Common.Utilities;
using TestASP.Model;

namespace TestASP.BlazorServer.Services
{
    public class UserService : BaseApiService, IUserService
	{

        public UserService(
            IHttpClientFactory httpClient,
            ILogger<UserService> logger,
            ConfigurationManager configuration,
            ProtectedLocalStorage localStorage)
            : base(httpClient, logger, configuration, localStorage)
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

