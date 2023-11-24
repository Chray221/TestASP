using System;
using System.Collections.ObjectModel;
using TestASP.Common.Extensions;

namespace TestASP.Data.ChurchTools
{
    public class SongList : ObservableCollection<Song>
    {
        public string Name { get; set; }

        public SongList(string name) : base()
        {
            Name = name;
        }


        public SongList(List<Song> songs, string name) : base(songs)
        {
            Name = name;
        }

        public SongList(IEnumerable<Song> songs, string name) : base(songs)
        {
            Name = name;
        }

        public SongList Search(string? searchKey)
        {
            if(!string.IsNullOrEmpty(searchKey))
            {
                //string[] searchKeys = searchKey.Split(" ");
                //return new SongList(
                //    this.Where(song =>
                //    {
                //        int.TryParse(searchKey, out int pageNumber);

                //        return (pageNumber >= 0 && song.Page == pageNumber) ||
                //               song.Lyrics.ContainsLower(searchKey) ||
                //               searchKeys.Any(key => song.Lyrics.ContainsLower(key));
                //    }).OrderBy( song => {
                //        int count = song.Lyrics.ContainsLower(searchKey) ? 1 : 0;
                //        count += searchKeys.Count(key => song.Lyrics.ContainsLower(key));
                //        return count;
                //    }), "Searched Songs");

                //return
                searchKey = searchKey.ToLower();
                return new SongList(this.Where(song =>
                {
                    var lyrics = (song.Lyrics ?? "").ToLower();
                    var title = (song.Title ?? "").ToLower();
                    if (song.Page.ToString() == searchKey)
                        return true;
                    else if (!string.IsNullOrEmpty(song.Lyrics) && lyrics.ContainsLower(searchKey))
                        return true;
                    else if (!string.IsNullOrEmpty(song.Title) && title.ContainsLower(searchKey))
                        return true;

                    return false;
                }), "Searched Songs");
            }
            return this;
        }
    }

}

