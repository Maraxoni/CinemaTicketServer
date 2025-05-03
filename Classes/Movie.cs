using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CinemaTicketServer.Classes
{
    [DataContract]
    public class Movie
    {
        [DataMember]
        private static int lastId = 0;
        [DataMember]
        public int MovieID { get; private set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Director { get; set; }
        [DataMember]
        public List<string> Actors { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public byte[] Poster { get; set; }

        [JsonConstructor]
        public Movie(string title, string director, List<string> actors, string description, byte[] poster = null)
        {
            MovieID = ++lastId;
            Title = title;
            Director = director;
            Actors = actors;
            Description = description;
            Poster = poster;
        }
    }
}
