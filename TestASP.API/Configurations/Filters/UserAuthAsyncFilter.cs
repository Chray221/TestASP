using System;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.Filters;
using TestASP.Common.Extensions;
using TestASP.Core.IRepository;
using TestASP.Data;

namespace TestASP.API.Configurations.Filters
{
	public class UserAuthAsyncFilter : IAsyncAuthorizationFilter
    {
        private ILogger _logger { get; }
        private readonly IUserRepository _userRepository;

        public UserAuthAsyncFilter(
            ILogger<UserAuthAsyncFilter> logger,
            IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            _logger.LogMessage($"[{context.ActionDescriptor.DisplayName}]: API: {context.HttpContext.Request.Path} : ModelState IsValid: {context.ModelState.IsValid}");
            IIdentity? userIdentity = context.HttpContext.User.Identity;
            if (userIdentity?.IsAuthenticated ?? false)
            {
                User? loggedInUser = await _userRepository.GetAsync(userIdentity.Name ?? "");
                if (loggedInUser != null)
                {
                    context.HttpContext.Items["LoggedInUser"] = loggedInUser;
                }
                //else
                //{
                //    //context.Result = new ForbidResult();
                //}
            }
        }
    }
}

