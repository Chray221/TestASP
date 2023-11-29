using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TestASP.API.Configurations;
using TestASP.API.Configurations.Filters;
using TestASP.Model;
using TestASP.Configurations;
using TestASP.Domain.Configurations;
using TestASP.API.Models;

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

Setting.Current.Init(builder.Environment, builder.Configuration);

builder.Services.RegisterAPIRepository()
                .RegisterRepositories()
                .RegisterServices()
                .RegisterAuthentication(builder.Configuration);
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

