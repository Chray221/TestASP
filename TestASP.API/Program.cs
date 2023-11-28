using Microsoft.AspNetCore.Mvc;
using TestASP.API.Configurations;
using TestASP.API.Configurations.Filters;
using TestASP.Configurations;
using TestASP.Domain.Configurations;

var builder = WebApplication.CreateBuilder(args);

// add Serilog logging config
//var logger = new LoggerConfiguration()
//    .WriteTo.Console()
//    .WriteTo.File("Logs/NzWalks_Log.txt", rollingInterval: RollingInterval.Minute)
//    .MinimumLevel.Warning()
//    .CreateLogger();

//builder.Logging.ClearProviders();
//builder.Logging.AddSerilog(logger);

// Add services to the container.
builder.Services.AddSingleton(builder.Configuration);

builder.Services.RegisterAPIRepository()
                .RegisterRepositories()
                .RegisterServices();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddControllers(option =>
{
    //option.ReturnHttpNotAcceptable = true;
    option.Filters.Add<ControllerExceptionFilter>();
    option.Filters.Add<UserAuthAsyncFilter>();
})
    // to add Accept: application/xml
    .AddXmlDataContractSerializerFormatters();

// NOTE: add default versioning
builder.Services.AddApiVersioning(cfg =>
{
    cfg.DefaultApiVersion = new ApiVersion(1, 0);
    cfg.AssumeDefaultVersionWhenUnspecified = true;
});



builder.Services.AddSwaggerConfig();

builder.Services.AddDatabaseConfig();
builder.Services.AddIdentityService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfig();
}

//app.AddIdentity();

app.UseHttpsRedirection();

app.UseAuthentication();
//app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();

app.Run();

