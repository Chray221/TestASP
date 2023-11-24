using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestASP.Data.Social
{
	public class PostImage: BaseData
	{
		public int PostId { get; set; }
		public int ImageId { get; set; }
		#region ForeignKeys
		[ForeignKey(nameof(PostId))]
		public Post? Post { get; set; }
		[ForeignKey(nameof(ImageId))]
		public ImageFile? Image { get; set; }
		#endregion
		public PostImage()
		{
		}
	}
}

