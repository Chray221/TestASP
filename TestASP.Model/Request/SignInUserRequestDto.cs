

using System.ComponentModel.DataAnnotations;

namespace TestASP.Model
{
    public class SignInUserRequestDto
    {
        [Required, MinLength(6, ErrorMessage = "Username is too short")]
        public string Username { get; set; }
        [Required, MinLength(8,ErrorMessage = "Password is too short")]
        public string Password { get; set; }
        public SignInUserRequestDto()
        {
        }
    }
}
