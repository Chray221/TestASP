using System;
using TestASP.Common.Helpers;
using static System.Net.WebRequestMethods;

namespace TestASP.Data
{
	public class ImageFile : BaseData
    {
        public string Url { get; set; }
        public string ThumbUrl { get; set; }

        public ImageFile() : base()
        {

        }

        public static ImageFile Mock()
        {
            return new ImageFile() {
                //Id = Guid.NewGuid(),
                Id = RandomizerHelper.GetRandomInt(1, 1000),
                Url = RandomizerHelper.GetRandomImage(),
                ThumbUrl = RandomizerHelper.GetRandomImage()
            };
        }
    }
}