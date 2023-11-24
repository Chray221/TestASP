using System;
using System.ComponentModel.DataAnnotations.Schema;
using TestASP.Common.Extensions;
using TestASP.Common.Helpers;
//using TestASP.Models;
//using TestASP.Models.Social;

namespace TestASP.Data.Social
{
	public class PostComment : BaseData
	{
		public int PostId { get; set; }
		public string Comment { get; set; }
		public int UserId { get; set; }

		#region ForeignKeys

		[ForeignKey(nameof(PostId))]
		public Post? Post { get; set; }

		[ForeignKey(nameof(UserId))]
		public User? User { get; set; }

		public List<PostCommentReply> CommentReplies { get; set; }

		#endregion
		public PostComment()
		{
		}

        //public override PostCommentDto ToDto()
        //{
        //    return new PostCommentDto(this);
        //}

		public static PostComment Mock(int? postId = null)
		{
			User userMock = User.Mock();
			return new PostComment()
			{
				//Id = Guid.NewGuid(),
				Id = RandomizerHelper.GetRandomInt(1, 1000),
				PostId = postId ?? RandomizerHelper.GetRandomInt(1,1000),
                Comment = RandomizerHelper.GetRandomName(RandomizerHelper.GetRandomInt(15, 20)),
                //UserId = Guid.Parse(userMock.Id),
                UserId = userMock.Id,
                User = userMock,
                CommentReplies = RandomizerHelper.GetRandomInt(1, 6).Select((x) => PostCommentReply.Mock()).ToList()
            };
        }
    }
}

