using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using TestASP.Common.Extensions;

namespace TestASP.API.Configurations.Filters
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class TestMiddleware
    {
        private readonly RequestDelegate _next;
        public readonly ILogger _logger;

        public TestMiddleware(RequestDelegate next, ILogger<TestMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public Task Invoke(HttpContext httpContext)
        {
            _logger.LogMessage("Test1");
            return _next(httpContext);
        }
    }

    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class Test2Middleware
    {
        private readonly RequestDelegate _next;
        public readonly ILogger _logger;

        public Test2Middleware(RequestDelegate next, ILogger<Test2Middleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public Task Invoke(HttpContext httpContext)
        {
            _logger.LogMessage("Test2");
            return _next(httpContext);
        }
    }

    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class Test3Middleware
    {
        private readonly RequestDelegate _next;
        public readonly ILogger _logger;

        public Test3Middleware(RequestDelegate next, ILogger<Test3Middleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public Task Invoke(HttpContext httpContext)
        {
            _logger.LogMessage("Test3");
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class TestMiddlewareExtensions
    {
        public static IApplicationBuilder UseTestMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TestMiddleware>()
                          .UseMiddleware<Test2Middleware>()
                          .UseMiddleware<Test3Middleware>();
        }
    }
}

