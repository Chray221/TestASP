using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TestASP.Model;

namespace TestASP.Web.Models.ViewModels;

public class LoginViewModel 
{   
    [Required, MinLength(5, ErrorMessage = "Username is too short")]
    public string Username { get; set; }
    [Required, MinLength(8,ErrorMessage = "Password is too short")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DisplayName("Remember Me?")]
    public bool IsRememberMe { get; set; }
}
