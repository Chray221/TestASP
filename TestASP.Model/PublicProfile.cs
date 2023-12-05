using System;
using System.IO;
using TestASP.Data;

namespace TestASP.Model
{
    public class PublicProfile : BaseDto
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }

        public PublicProfile() { }
        public PublicProfile(User user, string? rootUrl)
        {
            //rootUrl = Setting.Current.BaseUserImagePath;
            //Id = Guid.Parse(user.Id);
            Id = user.Id;
            FirstName = user.FirstName;
            MiddleName = user.MiddleName;
            LastName = user.LastName;
            Image = user.Image;

            if (string.IsNullOrEmpty(Image))
            {
                Image = "Image/Logo.png";
            }
            if (!string.IsNullOrEmpty(Image) && !Image.Contains(rootUrl) &&
                !Image.StartsWith("http") && !Image.StartsWith("https"))
            {
                 Image = Path.Combine(rootUrl, Image);
                //Image = Setting.Current.GetUserFileUrl(Image);
            }
        }

        public T UpdateImagePath<T>(string rootUrl) where T : PublicProfile
        {
            return (T)this;
        }
    }
}
