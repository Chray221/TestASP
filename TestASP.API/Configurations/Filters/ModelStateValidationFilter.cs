using System;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using TestASP.API.Extensions;
using TestASP.Common.Extensions;
using TestASP.API.Helpers;
using TestASP.Model;

namespace TestASP.API.Configurations.Filters
{
    public class ModelStateValidationFilter : IActionFilter
    {
        public ILogger _logger { get; }

        public ModelStateValidationFilter(ILogger<ModelStateValidationFilter> logger)
		{
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogMessage($"[{context.Controller.GetType().Name}]: API Ended: {context.HttpContext.Request.Path} : ModelState IsValid: {context.ModelState.IsValid}");
            if (!context.ModelState.IsValid)
            {
                context.Result = MessageHelper.BadRequest(context.ModelState);
            }
            else
            {
                if(context.Result is OkObjectResult okResult && okResult.Value is not ResultBase)
                {
                    if(okResult.Value is string valueStr)
                    {
                        context.Result = MessageHelper.Ok(valueStr);
                    }
                    else
                    {
                        context.Result = MessageHelper.Ok(okResult.Value, "Success");
                    }
                }
                if(context.Result is ObjectResult objectResult && objectResult.Value is not ResultBase)
                {
                    if (objectResult.Value is string valueStr)
                    {
                        context.Result = MessageHelper.Error(valueStr, objectResult.StatusCode ?? StatusCodes.Status500InternalServerError);
                    }
                    else if (objectResult.StatusCode == StatusCodes.Status200OK)
                    {
                        context.Result = MessageHelper.Ok(objectResult.Value, "Success");
                    }
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogMessage($"[{context.Controller.GetType().Name}]: API Starting: {context.HttpContext.Request.Path} : ModelState IsValid: {context.ModelState.IsValid}");
            //if (context.ModelState.IsValid == false)
            //{
            //    context.Result = new ObjectResult(context.ModelState)
            //    {
            //        StatusCode = StatusCodes.Status400BadRequest
            //    };
            //}
        }
    }
}

