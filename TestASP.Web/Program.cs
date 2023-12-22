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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Questionnaire}/{id?}/{action=Answer}/{userQuestionnaireId?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Questionnaire}/{action=SubQuestions}/{answer?}");
// app.MapControllerRoute(
//     name: "AnsweredQuestionnaire",
//     pattern: "Questionnaire/{id}/Answer/{anotherId?}");
// app.MapControllerRoute(
//     name: "NewAnsweredQuestionnaire",
//     pattern: "Questionnaire/{id}/Answer");
app.Run();
