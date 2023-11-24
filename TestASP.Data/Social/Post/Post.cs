using System;
using System.ComponentModel.DataAnnotations.Schema;
using TestASP.Common.Extensions;
using TestASP.Common.Helpers;
//using TestASP.Models;
//using TestASP.Models.Social;

namespace TestASP.Data.Social
{
	public class Post : BaseData
	{
		public string Title { get; set; }
		public string Content { get; set; }
		public Guid AuthorId { get; set; }
        public string Image { get; set; }

        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
		public int SharesCount { get; set; }

        #region ForeignKeys
        [ForeignKey(nameof(AuthorId))]
        public User? Author { get; set; }

        public List<PostImage> Images { get; set; }
        public List<PostComment> Comments { get; set; }
        public List<PostLike> Likes { get; set; }
        public List<PostShare> Shares { get; set; }
        #endregion

        public Post(): base()
        {
            Image = "";
            Title = "";
            Content = "";
            AuthorId = Guid.Empty;
            LikesCount = 0;
            SharesCount = 0;
            CommentsCount = 0;
            Author = null;
        }

        public Post(string title, string content, string image, Guid authorId) : base()
        {
            Title = title;
            Content = content;
            Image = image;
            AuthorId = authorId;
            LikesCount = 0;
            SharesCount = 0;
            CommentsCount = 0;
            Author = null;
        }

        public string GetTimelapse()
        {
            return CreatedAt.GetTimelapse();
        }

        //public override PostDto ToDto()
        //{
        //    return new PostDto(this);
        //}

        public Event ToEvent(DateTime dateStarted, DateTime? dateEnded = null)
        {
            return new Event( title: Title,
                content: Content,
                image: Image,
                AuthorId,
                dateStarted,
                dateEnded)
                {
                    Author = Author,
                    LikesCount = LikesCount,
                    SharesCount = SharesCount,
                    CreatedAt = CreatedAt
                };
        }

        public static Post Mock()
        {
            User authorMock = User.Mock();
            
            return new Post(title: "How To Manage Your Time & Get More Done",
            content: "This is a wider card with supporting text below as a natural lead-in to additional content. This content is a little bit longer.This is a wider card with supporting text below as a natural lead-in to additional content. This content is a little bit longer.This is a wider card with supporting text below as a natural lead-in to additional content. This content is a little bit longer.This is a wider card with supporting text below as a natural lead-in to additional content. This content is a little bit longer.This is a wider card with supporting text below as a natural lead-in to additional content. This content is a little bit longer.This is a wider card with supporting text below as a natural lead-in to additional content. This content is a little bit longer.",
            image: RandomizerHelper.GetRandomImage(),
            Guid.NewGuid())
            {
                Author = authorMock,
                LikesCount = RandomizerHelper.GetRandomInt(),
                SharesCount = RandomizerHelper.GetRandomInt(),
                CreatedAt = DateTime.Now.Add(TimeSpan.FromDays(Random.Shared.NextInt64(-60, 0)))
            };
        }
    }
}

