using System;
using System.ComponentModel.DataAnnotations.Schema;
using TestASP.Common.Helpers;
//using TestASP.Models.Social;

namespace TestASP.Data.Social
{
	public class PostCommentReply : PostComment
	{
		public int PostCommentId { get; set; }
		#region ForeignKeys

		[ForeignKey(nameof(PostCommentId))]
		public PostComment? PostComment { get; set; }

		#endregion
		public PostCommentReply()
		{
		}

		//public PostCommentReplyDto ToDto()
		//{
		//	return new PostCommentReplyDto(this);
		//}



        public static PostCommentReply Mock(int? postId = null, int? postCommentId = null)
        {
            User userMock = User.Mock();
            return new PostCommentReply()
            {
                //Id = Guid.NewGuid(),
                //PostId = postId ?? Guid.NewGuid(),
                Id = RandomizerHelper.GetRandomInt(1, 1000),
                PostId = postId ?? RandomizerHelper.GetRandomInt(1, 1000),
                Comment = RandomizerHelper.GetRandomName(RandomizerHelper.GetRandomInt(15, 20)),
                //UserId = Guid.Parse(userMock.Id),
                UserId = userMock.Id,
                User = userMock,
                //PostCommentId = postCommentId ?? Guid.NewGuid()
                PostCommentId = postCommentId ?? RandomizerHelper.GetRandomInt(1, 1000)
                //CommentReplies = RandomizerHelper.GetRandomInt(1, 6).Select((x) => PostCommentReply.Mock()).ToList()
            };
        }
    }
}

