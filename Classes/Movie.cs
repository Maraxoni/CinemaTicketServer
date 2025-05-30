﻿using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CinemaTicketServer.Classes
{
    [DataContract]
    public class Movie
    {
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

        public Movie(string title, string director, List<string> actors, string description, byte[] poster = null)
        {
            MovieID = ++lastId;
            Title = title;
            Director = director;
            Actors = actors;
            Description = description;
            Poster = poster;
        }

        [JsonConstructor]
        public Movie(int movieID, string title, string director, List<string> actors, string description, byte[] poster)
        {
            MovieID = movieID;
            Title = title;
            Director = director;
            Actors = actors;
            Description = description;
            Poster = poster;

            if (movieID > lastId)
            {
                lastId = movieID;
            }
        }
    }
}