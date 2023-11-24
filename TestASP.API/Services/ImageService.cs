//using System.Web;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Hosting; //*get RootPath
//using System.Drawing;               //CoreCompat Image handling
//using System.Drawing.Drawing2D;     //CoreCompat Image handling
//using System.Drawing.Imaging;       //CoreCompat Image handling
//using Microsoft.AspNetCore.Authorization;   //Identity: Register,Login
//using Microsoft.AspNetCore.Authentication;  //Identity: Register,Login
//using Microsoft.AspNetCore.Identity;        //Identity: Register,Login
//using System.Linq;

//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.Processing;
//using SixLabors.ImageSharp.PixelFormats;
//using TestAPI.Controllers;
//using TestAPI.ApiControllers;
//using TestAPI.Data;
using Microsoft.AspNetCore.Components.Forms;
using TestASP.API.Models;
using TestASP.Data;
using TestASP.Common.Extensions;

namespace TestASP.API.Services
{
    public class ImageService
	{
        private const string ImagePath = "Image";
        private const string FolderRootPath = "Upload";
        private const string UserImageForlderPath = $"Upload/User";
        private const string ImageFolderPath = $"Upload/Image";

        private IWebHostEnvironment _webHostEnvironment { get; }
        private ILogger _logger { get; }

        /// <summary>
        /// applicationRootPath/Upload/Image
        /// </summary>
        // private string BaseImagePath { get; }
        /// <summary>
        /// applicationRootPath/Upload/User
        /// </summary>
        // private string BaseUserImagePath { get; }

        public ImageService(IWebHostEnvironment webHostEnvironment, ILogger<ImageService> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            // BaseImagePath = Path.Combine(_webHostEnvironment.ContentRootPath, ImageFolderPath);
            // BaseUserImagePath = Path.Combine(_webHostEnvironment.ContentRootPath, UserImageForlderPath);
        }

        public bool IsImageExist(string imagePath)
        {
            return File.Exists(imagePath);
        }

        public byte[] GetImageByte(string imagePath)
        {
            return File.ReadAllBytes(GetImageString(imagePath));
        }

        public string GetImageString(string imagePathName)
        {
            return Path.Combine(Setting.Current.BaseImagePath, imagePathName.Replace(Setting.Current.BaseImagePath, ""));
        }

        public async Task<bool> SaveImageAsync(IFormFile postedFile, string filePath)
        {
            ////Extract Image File Name.
            //string fileName = System.IO.Path.GetFileName(postedFile.FileName);

            ////Set the Image File Path.
            //string filePath = "~/Uploads/" + fileName;

            ////Save the Image File in Folder. USING ENTITY
            //postedFile.SaveAs(Server.MapPath(filePath));
            if (postedFile.Length > 0)
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await postedFile.CopyToAsync(fileStream);
                }
            }
            return true;
        }

        //public void SaveThumbImage(IFormFile postedFile, string thumbFilePath)
        //{
        //    SaveResizedImage(postedFile, thumbFilePath, 200);
        //}

        //public void SaveResizedImage(IFormFile postedFile, string thumbFilePath, int newWidth)
        //{

        //    // Image.Load(string path) is a shortcut for our default type. 
        //    // Other pixel formats use Image.Load<TPixel>(string path))
        //    var imageInfo = Image.Identify(postedFile.OpenReadStream());

        //    double dblWidth_origial = imageInfo.Width;
        //    double dblHeigth_origial = imageInfo.Height;
        //    double relation_heigth_width = dblHeigth_origial / dblWidth_origial;
        //    int newHeight = (int)(newWidth * relation_heigth_width);

        //    using (Image image = Image.Load(postedFile.OpenReadStream()))
        //    {
        //        image.Mutate(x => x
        //             .Resize(newWidth, newHeight));
        //        image.Save(thumbFilePath); // Automatic encoder selected based on extension.
        //    }
        //}

        #region User Image
        public async Task<bool> SaveUserImageAsync(IFormFile postedFile, string imagePathName, string thumbImagePathName)
        {
            string imagePath = Path.Combine(UserImageForlderPath, imagePathName);
            string thumbImagePath = Path.Combine(UserImageForlderPath, thumbImagePathName);
            if (await SaveImageAsync(postedFile, imagePath))
            {
                //SaveThumbImage(postedFile, thumbImagePath);
                return true;
            }

            return false;
        }

        public byte[] GetUserImageAsync(string imagePathName)
        {
            string imagePath = Path.Combine(UserImageForlderPath, imagePathName);
            return File.ReadAllBytes(GetUserImageString(imagePath));
        }

        private string GetUserImageString(string imagePathName)
        {
            return Path.Combine(Setting.Current.BaseUserImagePath, imagePathName.Replace(Setting.Current.BaseUserImagePath, ""));
        }

        private string GetSaveImageString(string imagePathName)
        {
            if (imagePathName.Contains($"{ImagePath}{Path.DirectorySeparatorChar}"))
            {
                return Path.Combine(UserImageForlderPath, imagePathName);
            }
            return Path.Combine(UserImageForlderPath, ImagePath, imagePathName);
        }


        public async Task<ImageFile?> SaveUserImageAsync(IBrowserFile file, Guid userId)
        {
            ////Extract Image File Name.
            //string fileName = System.IO.Path.GetFileName(postedFile.FileName);

            ////Set the Image File Path.
            //string filePath = "~/Uploads/" + fileName;

            ////Save the Image File in Folder. USING ENTITY
            //postedFile.SaveAs(Server.MapPath(filePath));
            try
            {
                if (userId != Guid.Empty)
                {

                    string fileExtension = Path.GetExtension(file.Name);
                    var filePath = GetUserImagePath(userId, $"image{fileExtension}");
                    CreateFolder(Path.Combine(Setting.Current.BaseUserImagePath, filePath.FolderPath));
                    if (file.Size > 0)
                    {
                        using (var fileStream = new FileStream(filePath.FullImagePath, FileMode.Create))
                        {
                            await file.OpenReadStream().CopyToAsync(fileStream);
                        }
                    }

                    ImageFile newImageFile = new ImageFile()
                    {
                        Url = filePath.ImagePath,
                        ThumbUrl = filePath.ImagePath
                    };
                    return newImageFile;
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
            return null;
        }
        #endregion

        public ImageFile CreateUserImageFile(Guid userId)
        {
            if (userId != Guid.Empty)
            {
                ImageFile newImageFile = new ImageFile();
                string userImageFolderPath = GetSaveImageString($"{userId}");
                string imagePath = Path.Combine(ImagePath, $"{userId}", $"{newImageFile.Id}");
                CreateFolder(userImageFolderPath);
                newImageFile.Url = $"{imagePath}.jpg";
                newImageFile.ThumbUrl = $"{imagePath}_thumb.jpg";
                return newImageFile;
            }
            return null;
        }

        #region Utils
        private void CreateFolder(string folderPath)
        {
            var directory = Directory.CreateDirectory(folderPath);
            _logger.LogMessage($"DIRECTORY ? {directory == null} {folderPath}");
            if (directory != null)
                _logger.LogMessage($"DIRECTORY {directory.FullName}");
        }

        private string GetFolderPath(string folderName)
        {
            return Path.Combine(FolderRootPath, folderName);
        }

        private (string ImagePath, string FolderPath, string FullImagePath) GetImagePath(string fileName)
        {
            string folderPath = Path.Combine(fileName.Replace(Path.GetFileName(fileName) , ""));
            string imagePath = Path.Combine(folderPath, fileName);
            string fullImagePath = Path.Combine(Setting.Current.BaseImagePath, imagePath);
            return (imagePath, folderPath, fullImagePath);
        }

        private (string ImagePath, string FolderPath, string FullImagePath) GetUserImagePath(Guid userId, string fileName)
        {
            string folderPath = Path.Combine(userId.ToString(), ImagePath);
            string imagePath = Path.Combine(folderPath, fileName);
            string fullImagePath = Path.Combine(Setting.Current.BaseUserImagePath, imagePath);
            return (imagePath, folderPath, fullImagePath);
        }
        #endregion
    }
}


