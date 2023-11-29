using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestASP.Model.ChurchSongs
{
    public class SongList : ObservableCollection<Song>
    {
        public string Name { get; set; }

        public SongList() : base()
        {

        }

        public SongList(IEnumerable<Song> songs) : base(songs)
        {

        }

        public SongList(List<Song> songs) : base(songs)
        {

        }

        public SongList(List<Song> songs, string name) : base(songs)
        {
            Name = name;
        }

        public SongList(IEnumerable<Song> songs, string name) : base(songs)
        {
            Name = name;
        }   
    }

    public class Song
    {
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [Range(1,9999, ErrorMessage = "Invalid Page field format.")]
        public int Page { get; set; }
        [Required]
        public string Lyrics { get; set; }
        [Required]
        public string Language { get; set; }
        public string TuneReference { get; set; }
        public string Writers { get; set; }
        public string Composers { get; set; }
        public string Topics { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [NotMapped]
        public List<string> LyricsList { get { return string.IsNullOrEmpty(Lyrics) ? new List<string>() : new List<string>(Lyrics.Split(new string[] { "\n\n" }, StringSplitOptions.None)); } }

        public Song()
        {
        }

        public void CreateId()
        {
            Language = string.IsNullOrEmpty(Language) ? "Bisaya" : Language;
            Id = $"{Language.Substring(0, 1)}{Page}";
            int count = -1;

            //do
            //{
            //    count++;
            //}
            //while (DataClass.GetInstance.Songs.Any(song => song.Id == (Id + (count <= 0 ? "" : $"{count}"))));
            Id += (count <= 0 ? "" : $"{count}");
        }

        public void Update(Song refSong)
        {
            UpdatedAt = DateTime.Now;
            Title = refSong.Title;
            Page = refSong.Page;
            Lyrics = refSong.Lyrics;
            Language = refSong.Language;
            TuneReference = refSong.TuneReference;
            Writers = refSong.Writers;
            Composers = refSong.Composers;
            Topics = refSong.Topics;
        }

    }
}

