

using Microsoft.AspNetCore.Mvc;
using TestASP.API.Helpers;

namespace TestASP.API.Controllers
{
    /// <summary>
    /// <br>[ApiController]</br>
    /// <br>[ApiVersion("1.0")]</br>
    /// <br>[Route("v{version:apiversion}/api/[controller]")]</br>
    /// </summary>
    public class MyControllerBase : ControllerBase
    {

        public string RootPath { get { return HostSetting.HostEnvironment?.ContentRootPath ?? HostSetting.HostEnvironment?.WebRootPath; } }
        public string RootUrl = "";

        public MyControllerBase(IWebHostEnvironment environment)
        {
            //Host.HostEnvironment = environment ?? throw new ArgumentNullException(nameof(environment));
            //this.Log($"PATH {RootPath}");
        }
    }
}
