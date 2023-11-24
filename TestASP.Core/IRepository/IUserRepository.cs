using System;
using System.Threading.Tasks;
using TestASP.Data;

namespace TestASP.Core.IRepository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetAsync(string username, string password);
        Task<User?> GetAsync(string username);
        Task<bool> IsUserNameExistAsync(string username);
    }
}
