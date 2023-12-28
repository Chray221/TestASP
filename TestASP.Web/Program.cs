using TestASP.Web.Configurations;
using TestASP.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(builder.Configuration);
builder.Services.RegisterServices();

builder.Services.AddControllersWithViews()
    .AddViewOptions(options =>
    {
        options.HtmlHelperOptions.ClientValidationEnabled = true;
    })
    .AddRazorOptions( options => 
    {
        options.ViewLocationFormats.Add("/{0}.cshtml");
    });
    
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.RegisterAuthConfig();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthConfig();

// default
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// questionnaire
app.MapControllerRoute(
    // name: "default",
    name: "existing answered questionnaire",
    pattern: "{controller=Questionnaire}/{id?}/{action=Answer}/{userQuestionnaireId?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Questionnaire}/{action=SubQuestions}/{answer?}");

// area
app.MapAreaControllerRoute(
    name: "Admin area",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
);
app.MapAreaControllerRoute(
    name: "Admin Questionnaire",
    areaName: "Admin",
    pattern: "Admin/{controller=Questionnaire}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "area default",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
//admin
// app.MapControllerRoute(
//     name: "Admin",
//     pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

    
// app.MapControllerRoute(
//     name: "Admin Update Questionnaire",
//     pattern: "Admin/{controller=Questionnaire}/{action=Index}/{id?}");

app.Run();
