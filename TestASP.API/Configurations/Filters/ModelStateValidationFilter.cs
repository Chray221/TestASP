using System;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using TestASP.API.Extensions;
using TestASP.Common.Extensions;

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
            _logger.LogMessage($"[{context.Controller.GetType().Name}]: API: {context.HttpContext.Request.Path} : ModelState IsValid: {context.ModelState.IsValid}");
            if (context.ModelState.IsValid == false)
            {
                context.Result = new ObjectResult(context.ModelState)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogMessage($"[{context.Controller.GetType().Name}]: API: {context.HttpContext.Request.Path} : ModelState IsValid: {context.ModelState.IsValid}");
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

