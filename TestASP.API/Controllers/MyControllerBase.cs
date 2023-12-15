

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestASP.API.Extensions;
using TestASP.API.Helpers;
using TestASP.API.Migrations;
using TestASP.Core.IRepository;
using TestASP.Data;
using TestASP.Data.Questionnaires;
using TestASP.Model.Questionnaires;

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


        /// <summary>
        /// return Unauthorize if User is null and not verified from UserAuthAsyncFilter using JWT Token
        /// <br>return BadRequest if ModelState.IsValid == false</br>
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        internal Task<IActionResult> VerifyLogin(Func<User, Task<IActionResult>> func)
        {
            User? user = this.GetLoggedInUser();
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    return func.Invoke(user);
                }
                return Task.FromResult(MessageHelper.BadRequest(ModelState));
            }
            return Task.FromResult(MessageHelper.Unauthorized("Unauthorized access."));
        }

        /// <summary>
        /// return BadRequest if ModelState.IsValid == false
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        //internal Task<IActionResult> VerifyModelState(Func<Task<IActionResult>> func)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        return func.Invoke();
        //    }
        //    return Task.FromResult(MessageHelper.BadRequest(ModelState));
        //}
    }

    /// <summary>
    /// <br>[ApiController]</br>
    /// <br>[ApiVersion("1.0")]</br>
    /// <br>[Route("v{version:apiversion}/api/[controller]")]</br>
    /// </summary>
    public class MyRepositoryControllerBase<TRepository,TRepositoryModel> : MyControllerBase
        where TRepository: IBaseRepository<TRepositoryModel>
        where TRepositoryModel : BaseData
    {
        public readonly TRepository _repository;
        public readonly IMapper _mapper;

        public string RootPath { get { return HostSetting.HostEnvironment?.ContentRootPath ?? HostSetting.HostEnvironment?.WebRootPath; } }
        public string RootUrl = "";

        public MyRepositoryControllerBase(
           IWebHostEnvironment environment,
           TRepository repository)
            : base(environment)
        {
            _repository = repository;
        }

        public MyRepositoryControllerBase(
            IWebHostEnvironment environment,
            TRepository repository,
            IMapper mapper)
            : base(environment)
        {
            _repository = repository;
            _mapper = mapper;
        }
    }
}
