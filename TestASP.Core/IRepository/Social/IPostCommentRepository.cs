using System;
using TestASP.Core.IRepository;
using TestASP.Data.Social;

namespace TestASP.Core.IRepository.Social
{
	public interface IPostCommentRepository : IBaseRepository<PostComment>
	{
        Task<List<PostComment>> GetPostComments(Guid postId);
    }
}

