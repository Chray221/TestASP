using System;
using System.IO;
using TestASP.Data;

namespace TestASP.API.Models
{
    public class PublicProfile : BaseDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        public PublicProfile() { }
        public PublicProfile(Data.User user, string rootUrl)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Image = user.Image;

            if (string.IsNullOrEmpty(Image))
            {
                Image = "Image/Logo.png";
            }
            if (!string.IsNullOrEmpty(Image) && !Image.Contains(rootUrl))
            {
                Image = Path.Combine(rootUrl, Image);
            }
        }

        public T UpdateImagePath<T>(string rootUrl) where T : PublicProfile
        {
            return (T)this;
        }
    }
}
