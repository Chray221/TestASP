using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TestASP.Common.Helpers;

namespace TestASP.Data
{
    [Index(nameof(Username), IsUnique = true)]
    public class User : BaseData
    //public class User : IdentityUser, IBaseModel<UserDto>, IBaseModel
    {
        [Required]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "middle_name")]
        public string MiddleName { get; set; }        
        [Required]
        public string Password { get; set; }
        [AllowNull]
        public int? ImageFileId { get; set; }
        [NotMapped]
        public bool IsOnline { get; set; }

        public string Email { get; set; }


        [ForeignKey("ImageFileId")]
        public virtual ImageFile? ImageFile { get; set; }

        [NotMapped]
        public string Image { get { return ImageFile?.ThumbUrl; } }

        //Guid IBaseModel<UserDto>.Id { get { return Guid.Parse(Id); } set { Id = value.ToString(); } }

        //Guid IBaseModel<BaseDto>.Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public User() : base()
        {

        }

        public User(string username, string firstName, string lastName, string password, string email) : base()
        {
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Email = email;
        }

        public object UserFormat(string rootURL = "")
        {
            return new
            {
                Id,
                Username,
                Image = rootURL + "/" + Image,
                FirstName,
                LastName
            };
        }

        public string GetName()
        {
            return $"{FirstName} {LastName}";
        }

        //public UserDto ToDto()
        //{
        //    return new UserDto(this);
        //}

        public static User Mock()
        {
            ImageFile mockImage = ImageFile.Mock();
            return new User()
            {
                FirstName = RandomizerHelper.GetRandomName(),
                LastName = RandomizerHelper.GetRandomName(),
                MiddleName = RandomizerHelper.GetRandomName(),
                ImageFileId = mockImage.Id,
                ImageFile = mockImage,
            };
        }
    }
}

