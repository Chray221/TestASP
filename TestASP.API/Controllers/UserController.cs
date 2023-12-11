using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestASP.API.Extensions;
using TestASP.API.Helpers;
using TestASP.Model;
using TestASP.Core.IRepository;
using TestASP.Data;
using AutoMapper;

namespace TestASP.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiversion}/api/[controller]")]
    public class UserController : MyControllerBase
    {
        #region fields
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        #endregion

        public UserController(
            IWebHostEnvironment environment,
            IUserRepository userRepository,
            IMapper mapper
            ) :base(environment)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("{idOrUsername}", Order = 0)]
        public async Task<IActionResult> Get([FromRoute] string idOrUsername)
        {
            Data.User? user = null;
            if (int.TryParse(idOrUsername,out int userId))
            {
                user = await _userRepository.GetAsync(userId);
            }
            else if (!string.IsNullOrEmpty(idOrUsername))
            {
                user = await _userRepository.GetAsync(idOrUsername);
            }

            if (user == null)
            {
                //return NotFound(MessageExtension.ShowCustomMessage("Not Found", "User not found"));
                return MessageHelper.NotFound("User not found");
            }

            //return Ok(_mapper.Map<PublicProfile>(user));
            return Ok(_mapper.Map<UserDto>(user));

        }
    }

}
