using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using TestASP.Model;
using TestASP.Web.IServices;
using TestASP.Web.Models.ViewModels;

namespace TestASP.Web;

public class AuthenticationController : BaseController
{
    public AuthenticationController(ILogger<AuthenticationController> logger)
        : base(logger)
    {

    }

    [HttpGet]
    public IActionResult Login([FromQuery] string? ReturnUrl)
    {
        LoginViewModel loginModel = new LoginViewModel();
        ViewBag.ReturnUrl = ReturnUrl;
        return View(loginModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> LoginAsync(
        LoginViewModel loginRequest,
        [FromQuery]string? ReturnUrl,
        [FromServices]IAuthService authService,
        [FromServices]IMapper mapper)
    {
        return TryCatch( async () =>
        {
            return await ApiResult( 
                loginRequest,
                await authService.LoginAsync(mapper.Map<SignInUserRequestDto>(loginRequest)), 
                async Data => 
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(Data.Token);

                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaims(jwt.Claims);
                    identity.AddClaim(new Claim("access-token", Data.Token));
                    var principal = new ClaimsPrincipal(identity);
                    var authProp = new AuthenticationProperties();
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    HttpContext.Session.SetString("JWTToken", Data.Token);
                    // return Redirect("Home");
                    if(!string.IsNullOrEmpty(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    return RedirectToAction("Index","Home");
                });
        });
        
    }
    
    [AllowAnonymous]
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(string? ReturnUrl)
    {
        await HttpContext.SignOutAsync();
        HttpContext.Session.Clear();
        return RedirectToAction("Login","Authentication", new {ReturnUrl});
    }
}
