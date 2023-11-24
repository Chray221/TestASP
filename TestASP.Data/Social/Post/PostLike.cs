using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestASP.Data.Social
{
	public class PostLike : BaseData
	{
		public int PostId { get; set; }
		public int UserId { get; set; }

		#region ForeignKeys

		[ForeignKey(nameof(UserId))]
		public User? User { get; set; }

		[ForeignKey(nameof(PostId))]
		public Post? Post { get; set; }

		#endregion
		public PostLike()
		{
		}
	}
}

