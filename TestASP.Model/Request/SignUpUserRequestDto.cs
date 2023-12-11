using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TestASP.Data;
using TestASP.Model.Helpers;

namespace TestASP.Model
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SignUpUserRequestDto : SignInUserRequestDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        public IFormFile? Image { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [EqualToValidation(nameof(Password), ErrorMessage = "Password mismatched")]
        public string ConfirmPassword { get; set; }

        public string? AddressStr { get; set; }
        public string? BirthPlaceStr { get; set; }
        public DateOnly? Birthdate { get; set; }

        public SignUpUserRequestDto()
        {
        }

        public User ToUser(ImageFile? imageFile)
        {
            return new User()
            {
                FirstName = FirstName,
                LastName = LastName,
                MiddleName = MiddleName,
                Email = Email,
                ImageFile = imageFile,
                Username = Username,
                Password = Password
            };
        }
    }
}
