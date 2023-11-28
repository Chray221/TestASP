# TestASP

# TestASP.API

## Auto Mapper

- ### Setup
    - add Package "AutoMapper"
    - create "MappingConfig"
        ``` csharp
        using AutoMapper;
        public class MappingConfig : Profile
        {
            public MappingConfig()
            {
                CreateMap<TFrom, TTo>()
                    // to create map From TTo to TFrom
                    .ReverseMap();
            }
        }
        ```
    - in `Program.cs`
        ```csharp    
        builder.Services.AddAutoMapper(typeof(MappingConfig));
        ```
- To Use
    - in `Controller`
        ```csharp
        IMapper _mapper {get;}
        public Controller(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ActionResult<List<DataDto>> GetData()
        {
            List<Data> list = _dataRepository.Get();
            return Ok(_mapper.Map<List<DataDto>>(list))
        }

        public ActionResult<DataDto> GetDataItem(int id)
        {
            Data item = _dataRepository.Get(id);
            return Ok(_mapper.Map<DataDto>(item))
        }
        ```


