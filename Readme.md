# TestASP

# TestASP.API

## [Auto Mapper](https://docs.automapper.org/en/stable/Getting-started.html)
- ### Setup
    - add Package "AutoMapper"
    - add Package "AutoMapper.Extensions.Microsoft.DependencyInjection"
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
        //before add controller
        builder.Services.AddControllers();
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

# Others

## EF Code First Migrations
- Using Terminal
    - create migration
        ```bash
        dotnet ef migrations add MigrationTitle --project ProjectName 
        ```
    - remove migration
        ```bash
        dotnet ef migrations remove
        ```
    - apply migration
        ```base
        dotnet ef database update
        ```

- Using Package Manager Host
    - create migration
        ```bash
        #if only one dbcontext
        Add-Migration MigrationTitle

        #if multiple dbcontext
        Add-Migration MigrationTitle --Context YourDBContextClassName

        #to remove migration
        Remove-Migration
        ```
    - remove migration
        ```bash
        #if only one dbcontext
        Remove-Migration

        #if multiple dbcontext
        Remove-Migration --Context YourDBContextClassName        
        ```
    - apply migration
        ```base
        Update-Database

        #if multiple dbcontext        
        Update-Database --Context YourDBContextClassName

        ```
## Visual Studio Pending Running
- show running
    ```bash
    ps
    ```

- kill running project
    ```bash
    kill -9 $(lsof -i:PORT -t) 2> /dev/null
    #e.g
    #my local api
    kill -9 $(lsof -i:7069 -t) 2> /dev/null
    #or
    #my local blazor server
    kill -9 $(lsof -i:7070 -t) 2> /dev/null
    ```


# For VSCode

- ## Build Solution
    ```bash
    dotnet build /Users/macbookpro/Documents/Projects/Reference/WebApp/TestASP/TestASP.sln /property:GenerateFullPaths=true
    ```
- ## Build API Project
    ```bash
    dotnet build /Users/macbookpro/Documents/Projects/Reference/WebApp/TestASP/TestASP.API/TestASP.API.csproj /property:GenerateFullPaths=true
    ```
- ## Add `launch.json` file in `.vscode` folder
    ```json
    {
        "version": "2.0.0",
        "tasks": [
            {
                "label": "build",
                "command": "dotnet",
                "type": "process",
                "args": [
                    "build",
                    "${workspaceFolder}/TestASP.sln",
                    "/property:GenerateFullPaths=true",
                    "/consoleloggerparameters:NoSummary;ForceNoAlign"
                ],
                "problemMatcher": "$msCompile"
            },
            {
                "label": "publish",
                "command": "dotnet",
                "type": "process",
                "args": [
                    "publish",
                    "${workspaceFolder}/TestASP.sln",
                    "/property:GenerateFullPaths=true",
                    "/consoleloggerparameters:NoSummary;ForceNoAlign"
                ],
                "problemMatcher": "$msCompile"
            },
            {
                "label": "watch",
                "command": "dotnet",
                "type": "process",
                "args": [
                    "watch",
                    "run",
                    "--project",
                    "${workspaceFolder}/TestASP.sln"
                ],
                "problemMatcher": "$msCompile"
            }
        ]
    }```

- ## Add `tasks.json` to `.vscode` folder (used in launch.json `preLaunchTask: $tasks.label`)
    ```json
    {
        "version": "2.0.0",
        "tasks": [
            {
                "label": "build",
                "command": "dotnet",
                "type": "process",
                "args": [
                    "build",
                    "${workspaceFolder}/TestASP.sln",
                    "/property:GenerateFullPaths=true",
                    "/consoleloggerparameters:NoSummary;ForceNoAlign"
                ],
                "problemMatcher": "$msCompile"
            },
            {
                "label": "publish",
                "command": "dotnet",
                "type": "process",
                "args": [
                    "publish",
                    "${workspaceFolder}/TestASP.sln",
                    "/property:GenerateFullPaths=true",
                    "/consoleloggerparameters:NoSummary;ForceNoAlign"
                ],
                "problemMatcher": "$msCompile"
            },
            {
                "label": "watch",
                "command": "dotnet",
                "type": "process",
                "args": [
                    "watch",
                    "run",
                    "--project",
                    "${workspaceFolder}/TestASP.sln"
                ],
                "problemMatcher": "$msCompile"
            }
        ]
    }
    ```

