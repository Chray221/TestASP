# TestASP

# TestASP.API

## Auto Mapper
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

