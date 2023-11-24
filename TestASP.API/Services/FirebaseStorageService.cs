//using System;
//using System.IO;
//using System.Threading.Tasks;
//using Firebase.Storage;
//using Microsoft.Extensions.Logging;
//using TestBlazor.Services.Context;
//using TestBlazor.Services.Contracts;

//namespace TestASP.API.Services
//{
//    public class FirebaseStorageService: IFirebaseStorageService
//    {
//        public ILogger<FirebaseStorageService> _logger { get; }

//        const string APIKEY = "AIzaSyA9DRQFIFZHJYymOCgvbaGMeOFNTcPNylw";
//        const string StorageBucket = "church-songs-317605.appspot.com";
//        private static FirebaseStorage Storage = new FirebaseStorage(StorageBucket);
//        static FirebaseStorageReference UsersStorage { get { return Storage.Child("Users"); } }
//        static FirebaseStorageReference DatabaseStorage { get { return Storage.Child("Database"); } }
//        static FirebaseStorageReference SongDBStorage { get { return DatabaseStorage.Child("song.db"); } }

//        //static IStorage Storage = CrossFirebaseStorage.Current.Instance;

//        //private static Task<string> OnAuthToken()
//        //{
//        //    return Task.FromResult(FirebaseHelper.Auth.GetAuth());
//        //}

//        public FirebaseStorageService(ILogger<FirebaseStorageService> logger)
//        {
//            _logger = logger;
//        }

//        public async Task<string> SaveUsersImage(int id, Stream imageStream)
//        {

//            //Guid imageName = Guid.NewGuid();
//            ////if(string.IsNullOrEmpty(filepath))
//            //var imageRef = UsersStorage.Child($"{id}").Child($"{imageName}");
//            //await imageRef.PutStreamAsync(imageStream);
//            //var image = (await imageRef.GetDownloadUrlAsync());
//            //App.Log($"SAVE TO {Storage.RootReference.Bucket}/Users/{id}/{imageName} => {image.OriginalString} => {image}");
//            //return image.OriginalString;

//            return null;


//            //return await UsersStorage.Child($"{id}").Child($"{imageName}{Path.GetExtension(filepath)}").PutAsync(imageStream);
//        }
            
//        public async Task<bool> UpdateSongDbAsync()
//        {
//            try
//            {
//                _logger.LogInformation($"SONG DB Path => {SongDBStorage}");
//                //App.Log($"SONG DB Path => {SongDBStorage.Path}");
//                using (Stream stream = new FileStream(SongDbContext.SongLocalPath, FileMode.Open))
//                {
//                    await SongDBStorage.PutAsync(stream);
//                }
//                using (Stream stream = new FileStream(SongDbContext.SongLocalPath, FileMode.Open))
//                {
//                        // save backup
//                            await DatabaseStorage.Child($"Song_{DateTime.Now:MMddyyyyhhmm}_buckup.db")
//                        .PutAsync(stream);
//                }
//                return true;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"{ex.Message}\n\n{ex.StackTrace}");
//                return false;
//            }
//        }
//    }
//}
