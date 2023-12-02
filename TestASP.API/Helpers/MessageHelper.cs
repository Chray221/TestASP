using System;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;
using TestASP.Model;

namespace TestASP.API.Helpers
{
    public static class MessageHelper
    {
        public static IActionResult Ok(string message)
        {
            return new OkObjectResult(ResultBase.Success(message));
        }

        public static IActionResult Ok<T>(T data, string message)
        {
            return new OkObjectResult(ResultBase.Success(data, message));
        }

        public static IActionResult BadRequest(string message)
        {
            return ToObjectResult(ResultBase.Error(message, StatusCodes.Status400BadRequest));
        }

        public static IActionResult BadRequest(ModelError error)
        {
            return ToObjectResult(ResultBase.Error(error));
        }

        public static IActionResult NotFound(string message)
        {
            return ToObjectResult(ResultBase.Error(message,StatusCodes.Status404NotFound));
        }

        public static IActionResult InternalServerError(string message)
        {
            return new ObjectResult(ResultBase.Error(message, StatusCodes.Status500InternalServerError))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        private static IActionResult ToObjectResult(ResultBase resultBase)
        {
            return new ObjectResult(resultBase)
            {
                StatusCode = resultBase.StatusCode
            };
        }
    }

    
}

