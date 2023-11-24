using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace TestASP.API.Helpers
{
    public static class HostSetting
    {
        public static IWebHostEnvironment HostEnvironment { get; set; }

        public static string GetRootUrl(this ControllerBase controllerBase)
        {
            var request = controllerBase.Request;
            if (request != null)
            {
                var host = request.Host.ToUriComponent();
                var pathBase = request.PathBase.ToUriComponent();
                return $"{request.Scheme}://{host}{pathBase}";
            }
            return string.Empty;
        }
    }
}
