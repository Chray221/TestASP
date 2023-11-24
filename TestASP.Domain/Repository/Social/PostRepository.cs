using Microsoft.Extensions.Logging;
using TestASP.Common.Extensions;
using TestASP.Common.Helpers;
using TestASP.Core.IRepository.Social;
using TestASP.Data.Social;
using TestASP.Domain.Contexts;
using TestASP.Domain.Repository;

namespace TestASP.Domain.Repository.Social
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
	{
        public PostRepository(TestDbContext dbContext, ILogger<PostRepository> logger) : base(dbContext, logger)
        {
            
        }

        public Task<List<Post>> GetUserRecentPostsAsync(Guid userId)
        {
            return TryCatch( async() =>
            {
                await Task.Delay(RandomizerHelper.GetRandomInt(1000, 3000));
                return RandomizerHelper.GetRandomInt(5, 10)
                                       .Select((index) => RandomizerHelper.GetRandomBoolean() ?
                                                            Post.Mock() :
                                                            Event.Mock())
                                       .ToList();
            });

            //return TryCatch(() => {
            //    return _entity.Include( post => post.Comments)
            //                  .Include(post => post.Likes)
            //                  .Include(post => post.Shares)
            //                  .Where(post => post.AuthorId == userId).ToListAsync();
            //});
        }


        public Task<List<Post>> GetRecentPostsAsync(Guid userId)
        {
            return TryCatch(async () =>
            {
                await Task.Delay(RandomizerHelper.GetRandomInt(1000, 3000));
                return RandomizerHelper.GetRandomInt(5, 10)
                                       .Select((index) => RandomizerHelper.GetRandomBoolean() ?
                                                            Post.Mock() :
                                                            Event.Mock())
                                       .ToList();
            });

            //return TryCatch(() => {
            //    return _entity.Include( post => post.Comments)
            //                  .Include(post => post.Likes)
            //                  .Include(post => post.Shares)
            //                  .Where(post => post.AuthorId == userId).ToListAsync();
            //});
        }

    }
}

