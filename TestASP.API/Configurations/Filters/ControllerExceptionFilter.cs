using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TestASP.API.Extensions;
using TestASP.API.Helpers;
using TestASP.Common.Extensions;

namespace TestASP.API.Configurations.Filters;

public class ControllerExceptionFilter : IExceptionFilter
{
    private readonly ILogger _logger;

    public ControllerExceptionFilter(ILogger<ControllerExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        Exception exception = context.Exception;

        if (exception != null)
        {
            _logger.LogException(exception);
#if DEBUG
            object content = exception;
#else
            object content = new { message = "Oops, something unexpected went wrong" };
#endif
            IActionResult result = new ObjectResult(content)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            if (exception is HttpResponseException httpResponseException)
            {
                //return StatusCode(httpResponseException.StatusCode, httpResponseException.Value);
                result = new ObjectResult(httpResponseException.Value)
                {
                    StatusCode = httpResponseException.StatusCode
                };
            }
            else if (exception is DbUpdateException dbUpdateException)
            {
                /* if (dbUpdateException.InnerException is SqlException sqlException)
                {
                    if (sqlException?.Number == 2627)
                    {
                        return Conflict(content);
                    }
                }
                else */ if (dbUpdateException.InnerException is SqliteException sqliteException)
                {
                    //if (sqliteException.SqliteErrorCode == 19)// Unique
                    //{
                    //    return BadRequest(content);
                    //}
                    //return BadRequest(content);
                    result = MessageHelper.BadRequest("");
                }
            }
            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}

public class HttpResponseException : Exception
{
    public HttpResponseException(int statusCode, object? value = null) =>
        (StatusCode, Value) = (statusCode, value);

    public int StatusCode { get; }

    public object Value { get; }
}

