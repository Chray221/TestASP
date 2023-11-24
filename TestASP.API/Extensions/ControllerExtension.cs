using System;
using TestASP.Data;

namespace TestASP.API.Extensions;

public static class ControllerExtension
{
    public static User? GetLoggedInUser(this HttpContext httpContext)
    {
        if (httpContext.Items.ContainsKey("LoggedInUser") &&
            httpContext.Items.TryGetValue("LoggedInUser", out object? userLoggedIn) &&
            userLoggedIn is User user)
        {
            return user;
        }
        return null;
    }
}

