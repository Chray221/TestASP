using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TestASP.API.Configurations.Attributes;
using TestASP.API.Helpers;
using TestASP.Data;

namespace TestASP.API.Models
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
        [FromForm]
        public IFormFile? Image { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [EqualToValidation(nameof(Password))]
        public string ConfirmPassword { get; set; }

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
