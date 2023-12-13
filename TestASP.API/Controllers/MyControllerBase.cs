

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestASP.API.Helpers;
using TestASP.Core.IRepository;
using TestASP.Data;

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
