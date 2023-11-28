using System;
using System.ComponentModel.DataAnnotations.Schema;
using TestASP.Common.Helpers;
//using TestASP.Models.Social;
using static System.Net.Mime.MediaTypeNames;

namespace TestASP.Data.Social
{
    public class Event : Post
    {
        public DateTime DateStarted { get; set; }
        public DateTime? DateEnded { get; set; }
        public string Venue { get; set; }
        public string VenueDetail { get; set; }

        [NotMapped]
        public List<User>? Participants { get; set; }

        public Event() : base()
        {
            DateStarted = DateTime.Now;
        }

        public Event(string title, string content, string image, int authorId, DateTime dateStarted, DateTime? dateEnded = null) : base(title, content, image, authorId)
        {
            DateStarted = dateStarted;
            DateEnded = dateEnded;
        }

        public int ParticipantsCount { get; set; }

        //public override PostEventDto ToDto()
        //{
        //    return new PostEventDto(this);
        //}

        public new static Event Mock()
        {
            Event eventMock = Post.Mock().ToEvent(RandomizerHelper.GetRandomDate(-30, 30),
                                                  RandomizerHelper.GetRandomDate(30, 40));
            eventMock.ParticipantsCount = RandomizerHelper.GetRandomInt();
            eventMock.Venue = RandomizerHelper.GetRandomName();
            eventMock.VenueDetail = RandomizerHelper.GetRandomName(RandomizerHelper.GetRandomInt(5,12));
            return eventMock;
        }

    }
}

