using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;
using TestASP.Data;
using TestASP.Model;
using TestASP.Model.Helpers;

namespace TestASP.BlazorServer.Models
{
	public class UserSignupRequest : SignInUserRequestDto
	{
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        public IBrowserFile? Image { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [EqualToValidation(nameof(Password), ErrorMessage = "Password mismatched")]
        public string ConfirmPassword { get; set; }

        public UserSignupRequest()
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

