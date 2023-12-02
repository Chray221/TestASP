using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

    public static ModelStateDictionary AddRuleFor<T, FieldDT>(
        this ModelStateDictionary modelState,
        T data,
        Expression<Func<T, FieldDT>> field,
        Func<FieldDT, bool> condition,
        string messageError)
    {
        if(!condition(field.Compile().Invoke(data)))
        {
            modelState.AddModelError(field.GetProperty(), messageError);
        }
        return modelState;
    }

    public static async Task<ModelStateDictionary> AddRuleForAsync<T, FieldDT>(
        this ModelStateDictionary modelState,
        T data,
        Expression<Func<T, FieldDT>> field,
        Func<FieldDT, Task<bool>> condition,
        string messageError)
    {
        if (!await condition(field.Compile().Invoke(data)))
        {
            modelState.AddModelError(field.GetProperty(), messageError);
        }
        return modelState;
    }

}


