

using Microsoft.AspNetCore.Mvc;
using TestASP.API.Helpers;

namespace TestASP.API.Controllers
{
    public class MyControllerBase : ControllerBase
    {

        public string RootPath { get { return HostSetting.HostEnvironment.ContentRootPath ?? HostSetting.HostEnvironment.WebRootPath; } }
        public string RootUrl = "";

        public MyControllerBase(IWebHostEnvironment environment)
        {
            //Host.HostEnvironment = environment ?? throw new ArgumentNullException(nameof(environment));
            //this.Log($"PATH {RootPath}");
        }
    }
}
