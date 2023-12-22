using System.Diagnostics;
using System.Web;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using TestASP.Web.Models;

namespace TestASP.Web;

public class BaseController : Controller
{
    ILogger _logger;

    public BaseController(ILogger logger)
    {
        _logger = logger;
    }

    IActionResult NavigagteTo(IActionResult view)
    {
        if (HttpContext.User.Identity?.IsAuthenticated != true)
        {
            // return RedirectToAction(actionName: "Index", controllerName: "Home");
            return RedirectToAction(actionName: "Login", controllerName: "Authentication");
        }
        return view;
    }

    public virtual async Task<IActionResult> TryCatch(Func<Task<IActionResult>> func)
    {
        try
        {
            return await func.Invoke();
        }
        catch (Exception ex)
        {
            return ToError(ex.Message);
        }
    }

    public virtual IActionResult TryCatch(Func<IActionResult> func)
    {
        try
        {
            return func.Invoke();
        }
        catch (Exception ex)
        {
            return ToError(ex.Message);
        }
    }

    internal IActionResult ToError(string? message)
    {
        return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = message });
    }

    public async Task<IActionResult> ApiResult<TRequest,T>(TRequest request, ApiResult<T> apiResult, Func<T,Task<IActionResult>> successResult)
    {
        if(!apiResult.IsSuccess)
        {
            if(apiResult.IsModelError)
            {
                foreach (var keyVal in apiResult.Errors)
                {
                    foreach (string error in keyVal.Value ?? Array.Empty<string>())
                    {
                        ModelState.AddModelError(keyVal.Key, error);
                    }
                }
                ViewBag.ErrorMessage = "One or more error in filed";
            }
            else if(apiResult.StatusCode == StatusCodes.Status401Unauthorized ||
                    apiResult.StatusCode == StatusCodes.Status403Forbidden)
            {
                return RedirectToAction("Logout","Authentication", new { ReturnUrl = Request.GetEncodedUrl()});
            }
            else
            {
                ViewBag.ErrorMessage = apiResult.Error;
            }
            
            return View(model: request);
        }
        return await successResult.Invoke(apiResult.Data);
    }
}
