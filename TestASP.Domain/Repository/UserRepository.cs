using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestASP.Core.IRepository;
using TestASP.Data;
using TestASP.Domain.Contexts;
using TestASP.Domain.Repository;

namespace TestASP.Domain.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        //UserManager<User> _userManager { get; }
        //internal TestDbContext _dbContext;
        //internal DbSet<User> _entity;
        //internal ILogger<UserRepository> _logger;

        //public UserRepository(
        //    TestDbContext dbContext,
        //    ILogger<UserRepository> logger,
        //    UserManager<User> userManager)
        //{
        //    _dbContext = dbContext;
        //    _logger = logger;
        //    _userManager = userManager;

        //    //_entity = _dbContext.Users;
        //    _entity = _dbContext.User;
        //}

        public UserRepository(TestDbContext dbContext, ILogger<BaseRepository<User>> logger) : base(dbContext, logger)
        {
        }

        // public Task<bool> DeleteAsync(Guid id)
        // {
        //     throw new NotImplementedException();
        // }

        public new Task<User?> GetAsync(int id)
        {
            //return WithUserImage().FirstOrDefaultAsync(user => user.Id == id.ToString());
            return WithUserImage().FirstOrDefaultAsync(user => user.Id == id);
        }

        public Task<User?> GetAsync(string username, string password)
        {
            return WithUserImage().FirstOrDefaultAsync(user => user.Username == username && user.Password == password);
        }

        public async Task<User?> GetAsync(string username)
        {
            return await WithUserImage().FirstOrDefaultAsync(user => user.Username == username);
        }

        public Task<List<User>> GetAsync(List<int> ids)
        {
            //IEnumerable<string> idsString = ids.Select(id => id.ToString());
            //return WithUserImage().Where(user => idsString.Contains(user.Id)).ToListAsync();
            return WithUserImage().Where(user => ids.Contains(user.Id)).ToListAsync();
        }

        public Task<bool> IsUserNameExistAsync(string username)
        {
            return WithUserImage().AnyAsync(user => user.Username == username);
        }

        //public async Task<bool> InsertAsync(User data)
        //{
        //    if (data == null)
        //    {
        //        throw new NullReferenceException($"Parammeter \"{typeof(User).Name}data\" is null in InsertAsync");
        //    }

        //    if (data != null)
        //    {
        //        if (data.Id == Guid.Empty.ToString())
        //        {
        //            data.Id = Guid.NewGuid().ToString();
        //        }
        //        data.CreatedAt = DateTime.Now;
        //        await _dbContext.AddAsync(data);
        //        await _dbContext.SaveChangesAsync();
        //        return true;
        //    }
        //    return false;
        //}

        //public async Task<bool> UpdateAsync(User data)
        //{

        //    if (data == null)
        //    {
        //        throw new NullReferenceException($"Parammeter \"{typeof(User).Name}data is null in UpdateAsync");
        //    }

        //    if (data != null)
        //    {
        //        data.IsDeleted = true;
        //        data.UpdatedAt = DateTime.Now;
        //        _dbContext.Update(data);
        //        await _dbContext.SaveChangesAsync();
        //        return true;
        //    }
        //    return false;
        //}

        IQueryable<User> WithUserImage()
        {
            //return _dbContext.Users.Include(user => user.ImageFile);
            return _entity.Include(user => user.ImageFile);
        }
    }
}
