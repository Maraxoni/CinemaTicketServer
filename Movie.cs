namespace CinemaTicketServer
{
    public class Movie
    {
        public string Title { get; set; }
        public string Director { get; set; }
        public List<string> Actors { get; set; }
        public string Description { get; set; }
        public byte[] Poster { get; set; }

        public Movie(string title, string director, List<string> actors, string description, byte[] poster = null)
        {
            Title = title;
            Director = director;
            Actors = actors;
            Description = description;
            Poster = poster;
        }
    }
}
