using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;


namespace CinemaTicketServer
{
    public class ServerMain
    {
        public static List<Movie> Movies { get; private set; } = new List<Movie>();
        public static List<Show> Shows { get; private set; } = new List<Show>();
        public static void Main(string[] args)
        {
            // Tworzymy przykładowe filmy
            var movie1 = new Movie(
                "Inception",
                "Christopher Nolan",
                new List<string> { "Leonardo DiCaprio", "Joseph Gordon-Levitt", "Elliot Page" },
                "A thief who steals corporate secrets through use of dream-sharing technology."
            );

            var movie2 = new Movie(
                "The Matrix",
                "The Wachowskis",
                new List<string> { "Keanu Reeves", "Laurence Fishburne", "Carrie-Anne Moss" },
                "A computer hacker learns from mysterious rebels about the true nature of his reality."
            );

            Movies.Add(movie1);
            Movies.Add(movie2);

            Shows.Add(new Show(movie1, DateTime.Today.AddHours(15), new TimeSpan(2, 0, 0), 100)); // Show lasts 2 hours
            Shows.Add(new Show(movie1, DateTime.Today.AddHours(19), new TimeSpan(2, 0, 0), 100)); // Show lasts 2 hours
            Shows.Add(new Show(movie2, DateTime.Today.AddHours(16), new TimeSpan(2, 0, 0), 120)); // Show lasts 2 hours, more seats
            Shows.Add(new Show(movie2, DateTime.Today.AddHours(20), new TimeSpan(2, 0, 0), 120)); // Show lasts 2 hours, more seats
            Shows.Add(new Show(movie1, DateTime.Today.AddDays(1).AddHours(14), new TimeSpan(2, 0, 0), 150)); // Show lasts 2 hours, more seats

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<IReservationService, ReservationService>();
            builder.Services.AddServiceModelServices();

            var app = builder.Build();

            //app.Urls.Add("http://192.168.50.183:8080");
            app.Urls.Add("http://localhost:8080");

            app.UseRouting();

            var binding = new BasicHttpBinding
            {
                MessageEncoding = WSMessageEncoding.Mtom
            };

            app.UseServiceModel(serviceBuilder =>
            {
                serviceBuilder.AddService<ReservationService>();
                serviceBuilder.AddServiceEndpoint<ReservationService, IReservationService>(binding, "/ReservationService");
            });

            app.Run();
        }
    }
}