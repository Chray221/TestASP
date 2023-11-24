using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TestASP.API.Helpers;

namespace TestASP.API.Models
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SignUpUserRequestDto : SignInUserRequestDto
    {
        [Required, BindProperty, FromFormSnakeCase]
        public string FirstName { get; set; }
        [Required, BindProperty, FromFormSnakeCase]
        public string LastName { get; set; }
        [BindProperty]
        public IFormFile Image { get; set; }
        [EmailAddress, BindProperty]
        public string Email { get; set; }

        public SignUpUserRequestDto()
        {
        }
    }
}
