using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestASP.API.Extensions;
using TestASP.API.Helpers;
using TestASP.Model;
using TestASP.Core.IRepository;
using TestASP.Data;

namespace TestASP.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiversion}/api/[controller]")]
    public class UserController : MyControllerBase
    {
        #region fields
        private IUserRepository _userRepository;
        #endregion

        public UserController( IWebHostEnvironment environment
            , IUserRepository userRepository) :base(environment)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{idOrUsername}", Order = 0)]
        public async Task<IActionResult> Get([FromRoute] string idOrUsername)
        {
            if(int.TryParse(idOrUsername,out int userId))
            {
                Data.User? user = await _userRepository.GetAsync(userId);
                if(user != null)
                {
                    return Ok(new PublicProfile(user,this.GetRootUrl()));
                }
            }
            else if (!string.IsNullOrEmpty(idOrUsername))
            {
                Data.User? user = await _userRepository.GetAsync(idOrUsername);
                if (user != null)
                {
                    return Ok(new PublicProfile(user, this.GetRootUrl()));
                }
            }

            //return NotFound(MessageExtension.ShowCustomMessage("Not Found", "User not found"));
            return MessageHelper.NotFound("User not found");
        }
    }

}
