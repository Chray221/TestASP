using System;
using System.IO;
using System.Threading.Tasks;

namespace TestASP.Core.IService
{
    public interface IFirebaseStorageService
    {

        Task<string> SaveUsersImage(int id, Stream imageStream);

        Task<bool> UpdateSongDbAsync();
    }
}
