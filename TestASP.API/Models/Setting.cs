using System;
using System.Web;

namespace TestASP.API.Models
{
	public class Setting
	{

        static Setting _instance;
        public static Setting Current { get { return _instance ??= new Setting(); } }

        public string FileUrl { get; private set; }
        public string APIRootUrl { get; private set; }
        /// <summary>
        /// applicationRootPath/Upload/Image
        /// </summary>
        public string BaseImagePath { get; private set; }
        /// <summary>
        /// applicationRootPath/Upload/User
        /// </summary>
        public string BaseUserImagePath { get; private set; }

        // privates
        private const string ImagePath = "Image";
        private const string FolderRootPath = "Upload";
        private const string UserImageForlderPath = $"Upload/User";
        private const string ImageFolderPath = $"Upload/Image";
        private IWebHostEnvironment _webHostEnvironment { get; set; }
        private Setting()
        {

        }

        public void Init(IWebHostEnvironment webHostEnvironment, ConfigurationManager configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            FileUrl = webHostEnvironment.ContentRootPath;
            BaseImagePath = Path.Combine(FileUrl, ImageFolderPath);
            BaseUserImagePath = Path.Combine(FileUrl, UserImageForlderPath);

            APIRootUrl = configuration["Urls:APIRootUrl"];
        }

        public string GetUserFileUrl(string filePath)
        {
            return GetRootFileUrl(Path.Combine(UserImageForlderPath, filePath));
        }

        public string GetFileUrl(string filePath)
        {
            return GetRootFileUrl(Path.Combine(ImageFolderPath, filePath));
        }

        private string GetRootFileUrl(string filePath)
        {
            // return Path.Combine(BaseUserImagePath,filePath);
            return Path.Combine(APIRootUrl, "resource", HttpUtility.UrlEncode(filePath));
        }
    }
}

