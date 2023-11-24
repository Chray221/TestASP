using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace TestASP.API.Models
{
    public class SignInUserRequestDto
    {
        [Required, BindProperty, MinLength(8, ErrorMessage = "Username is too short")]
        public string Username { get; set; }
        [Required, BindProperty, MinLength(8,ErrorMessage = "Password is too short")]
        public string Password { get; set; }
        public SignInUserRequestDto()
        {
        }
    }
}
