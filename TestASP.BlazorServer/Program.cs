using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TestASP.BlazorServer.Configurations;
using TestASP.BlazorServer.Data;

var builder = WebApplication.CreateBuilder(args);

JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    ContractResolver = new CamelCasePropertyNamesContractResolver()
};

// Add services to the container.
builder.Services.AddSingleton(builder.Configuration);
builder.Services.RegisterServices();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddAutoMapper(typeof(MappingConfig));


// Add this line of code
builder.Services.AddBootstrapBlazor();

builder.Services.RegisterAuthConfig();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthConfig();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

