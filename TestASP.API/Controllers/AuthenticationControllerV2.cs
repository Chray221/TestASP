using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TestASP.API.Extensions;
using TestASP.API.Helpers;
using TestASP.Model;
using TestASP.Common.Helpers;
using TestASP.Core.IRepository;
using TestASP.Core.IService;
using TestASP.Data;
using TestASP.Domain.Contexts;

namespace TestASP.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiversion}/api/[controller]")]
    public partial class AuthenticationController : MyControllerBase
    {
        private IUserRepository _userRepository;
        private IImageFileRepository _imageFileRepository;
        private IMapper _mapper;

        private TestDbContext _dbContext;
        public AuthenticationController(
            TestDbContext dbContext,
            IWebHostEnvironment environment,
            IUserRepository userRepository,
            IImageFileRepository imageFileRepository,
            IMapper mapper)
            :base(environment)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _imageFileRepository = imageFileRepository;
            _mapper = mapper;
        }

        // POST api/authentication/sign_in
        /// <summary>
        /// Sign in user using username and password json
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("sign_in")]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, Type = typeof(UserDto))]
        [SwaggerOperation(Summary = "Sign in user using username and password json")]
        public async Task<IActionResult> SignIn(
            [FromBody] SignInUserRequestDto user,
            [FromServices] IJwtSerivceManager jwtSerivceManager)
        {
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    Data.User userFound = await _userRepository.GetAsync(user.Username);
                    if(userFound == null)
                    {
                        //return BadRequest(MessageExtension.ShowCustomMessage("Sign In Error", "User is not registered", statusCode: HttpStatusCode.BadRequest));
                        return MessageHelper.BadRequest("User is not registered");
                    }

                    //if (SaltHasher.VerifyHash(user.Password, userFound.Password))
                    if(userFound.VerifyPassword(user.Password, userFound.Password))
                    {
                        UserDto userDto = new UserDto(userFound, this.GetRootUrl());
                        if (jwtSerivceManager.IsEnabled)
                        {
                            //userDto.Token = await jwtSerivceManager.CreateToken(user);
                            userDto.Token = jwtSerivceManager.CreateToken(userDto.ToData());
                            if (string.IsNullOrEmpty(userDto.Token))
                            {
                                //return StatusCode((int)HttpStatusCode.InternalServerError, MessageExtension.ShowCustomMessage("SignIn Error", "Something went wrong", "Okay", statusCode: HttpStatusCode.InternalServerError));
                                return MessageHelper.InternalServerError("Something went wrong in generating token");
                            }
                        }
                        return MessageHelper.Ok(userDto,"Successfuly login!.");
                    }


                    //return BadRequest(MessageExtension.ShowCustomMessage("SignIn Error", "Username or password mismatched", "Okay", statusCode: HttpStatusCode.BadRequest));
                    return MessageHelper.BadRequest("Username or password mismatched");
                }
                return MessageHelper.NotFound("User not found");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        //POST api/authentication/sign_up
        [HttpPut("sign_up")]
        [Consumes("application/json")]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, Type = typeof(UserDto))]
        public Task<IActionResult> SignUp(
            [FromBody]SignUpUserRequestDto user,
            [FromServices] IJwtSerivceManager jwtSerivceManager)
        {
            return RegisterUser(user, jwtSerivceManager);
        }

        [HttpPut("sign_up")]
        [HttpPut("sign_up/form")]
        [Consumes("multipart/form-data")]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, Type = typeof(UserDto))]
        public Task<IActionResult> SignUpForm(
            [FromForm] SignUpUserRequestDto user,
            [FromServices] IJwtSerivceManager jwtSerivceManager)
        {
            return RegisterUser(user, jwtSerivceManager, true);
        }

        private async Task<IActionResult> RegisterUser( SignUpUserRequestDto user, IJwtSerivceManager jwtSerivceManager, bool isFromForm = false)
        {
            if (user != null)
            {
                await ModelState.AddRuleForAsync(user, u => u.Username,
                        async (u) => !await _userRepository.IsUserNameExistAsync(user.Username),
                        "Username already taken.");
                ModelState.AddRuleFor(user, u => u.Image, img => isFromForm ? img != null : true, "Image is required");
            }
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    //if (jwtSerivceManager.IsEnabled)
                    //{
                    //    if (!await jwtSerivceManager.SaveIndentity(user))
                    //    {
                    //        return StatusCode((int)HttpStatusCode.InternalServerError, MessageExtension.ShowCustomMessage("Sign Up Error!", "Something went wrong!", statusCode: HttpStatusCode.InternalServerError));
                    //    }
                    //}

                    //Data.User newUser = new Data.User(user.Username, user.FirstName, user.LastName, SaltHasher.ComputeHash(user.Password), user.Email);
                    User newUser = _mapper.Map<User>(user);
                    //newUser.Password = SaltHasher.ComputeHash(user.Password);
                    newUser.Password = newUser.HashPassword(user.Password);
                    if (user.Image != null)
                    {
                        //IFormFile imageFile = user.Image;
                        //ImageFile newImage = ImageHelper.CreateUserImageFile(newUser.Id);
                        //if (imageFile != null &&
                        //        await _imageFileRepository.InsertAsync(newImage))
                        //{
                        //    if (await ImageHelper.SaveUserImageAsync(imageFile, newImage.Url, newImage.ThumbUrl))
                        //    {
                        //        newUser.ImageFileId = newImage.Id;
                        //        newUser.ImageFile = newImage;
                        //    }
                        //}
                        newUser.ImageFile = ImageHelper.CreateUserImageFile(newUser.Id);
                    }

                    if (await _userRepository.InsertAsync(newUser))
                    {
                        UserDto userDto = new UserDto(newUser, this.GetRootUrl());
                        if (jwtSerivceManager.IsEnabled)
                        {
                            //userDto.Token = await jwtSerivceManager.CreateToken(user);
                            userDto.Token = jwtSerivceManager.CreateToken(newUser);
                            if (string.IsNullOrEmpty(userDto.Token))
                            {
                                //return StatusCode((int)HttpStatusCode.InternalServerError, MessageExtension.ShowCustomMessage("Sign In Error", "Something went wrong", "Okay", statusCode: HttpStatusCode.InternalServerError));
                                return MessageHelper.InternalServerError("Something went wrong in generating token");
                            }
                        }
                        return Ok(userDto);
                    }
                    else
                    {
                        return MessageHelper.InternalServerError("Something went wrong in saving new user");
                    }
                }

                return BadRequest("User data not found.");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
