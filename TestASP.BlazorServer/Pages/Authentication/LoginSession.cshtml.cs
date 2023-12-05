using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using TestASP.Common.Helpers;
using TestASP.Model;

namespace TestASP.BlazorServer.Pages.Authentication
{
	public class LoginModel : PageModel
    {
        const string loginReturlUrl = "~/authentication/login";
        public async Task<IActionResult>
            OnGetAsync(string token, string? returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (string.IsNullOrEmpty(token))
            {
                return LocalRedirect(Url.Content("~/authentication/login"));
            }
            else
            { 
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaims(jwt.Claims);
                //identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "name").Value));
                //identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


                HttpContext.Session.SetString("JWTToken", token);
            }
            //return RedirectToAction("Index", "Home");
            return LocalRedirect(returnUrl);
        }
    }
}
