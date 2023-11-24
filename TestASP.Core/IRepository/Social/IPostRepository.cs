using System;
using TestASP.Data.Social;

namespace TestASP.Core.IRepository.Social
{
	public interface IPostRepository: IBaseRepository<Post>
	{
        Task<List<Post>> GetUserRecentPostsAsync(Guid userId);
        Task<List<Post>> GetRecentPostsAsync(Guid userId);
    }
}

