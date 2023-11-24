using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestASP.Data.Social
{
	public class PostShare : Post
	{
		public int PostId { get; set; }
		public int SharedById { get; set; }

		#region ForegnKeys

		[ForeignKey(nameof(SharedById))]
		public User SharedBy { get; set; }

		#endregion

		public PostShare()
		{
		}
	}
}

