using Microsoft.Extensions.Logging;
using TestASP.Common.Extensions;
using TestASP.Common.Helpers;
using TestASP.Core.IRepository.Social;
using TestASP.Data.Social;
using TestASP.Domain.Contexts;

namespace TestASP.Domain.Repository.Social
{
    public class PostCommentRepository : BaseRepository<PostComment>, IPostCommentRepository
	{
        public PostCommentRepository(TestDbContext dbContext, ILogger<PostCommentRepository> logger) : base(dbContext, logger)
        {
        }

        public Task<List<PostComment>> GetPostComments(Guid postId)
        {
            return TryCatch(async () =>
            {
                await Task.Delay(RandomizerHelper.GetRandomInt(1000, 3000));
                return RandomizerHelper.GetRandomInt(5, 10)
                                       .Select((index) => PostComment.Mock())
                                       .ToList();
            });
            //return TryCatch(() => );
        }
    }
}

