using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TestASP.BlazorServer.Pages.Authentication
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet(string? returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            
            HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            return LocalRedirect(returnUrl);
        }
    }
}
