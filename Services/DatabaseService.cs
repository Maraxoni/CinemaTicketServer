using CinemaTicketServer.Classes;
using System.Text.Json;
using System.Xml.Linq;

namespace CinemaTicketServer.Services
{
    public class DatabaseService : IDatabaseService
    {
        private List<Movie> movies;
        private List<Screening> screenings;
        private List<Account> accounts;
        private List<Reservation> reservations;

        private const string FolderPath = "Data";
        private const string MoviesFileName = "movies.json";
        private const string ScreeningsFileName = "screenings.json";
        private const string AccountsFileName = "accounts.json";
        private const string ReservationsFileName = "reservations.json";

        public DatabaseService()
        {
            movies = LoadFromFile<Movie>(MoviesFileName);
            screenings = LoadFromFile<Screening>(ScreeningsFileName);
            accounts = LoadFromFile<Account>(AccountsFileName);
            reservations = LoadFromFile<Reservation>(ReservationsFileName);

            //ChangePoster(1, "inception.jpg");
            //ChangePoster(2, "matrix.jpg");
            //ChangePoster(3, "interstellar.webp");
            //ChangePoster(4, "pulpfiction.jpg");
            //ChangePoster(5, "shawshank.jpg");
            //ChangePoster(6, "godfather.jpg");
            //ChangePoster(7, "fightclub.jpg");
            //ChangePoster(8, "forrestgump.jpg");
            //ChangePoster(9, "batman.jpg");
        }

        public void ChangePoster(int movieId, string fileName)
        {
            string name = Path.Combine("Data/", fileName);
            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            string defaultPosterPath = Path.Combine(projectDirectory, name);

            var movie = movies.FirstOrDefault(m => m.MovieID == movieId);

            Console.WriteLine(defaultPosterPath);

            if (File.Exists(defaultPosterPath) && movie != null)
            {
                byte[] imageBytes = File.ReadAllBytes(defaultPosterPath);

                movie.Poster = imageBytes;

                SaveMovies();
            }
            else
            {
                Console.WriteLine("File not found. ");
            }
        }

        public List<Movie> GetMovies() => movies;
        public void AddMovie(Movie movie)
        {
            movies.Add(movie);
            SaveMovies();
        }

        public List<Screening> GetScreenings() => screenings;
        public void AddScreening(Screening screening)
        {
            screenings.Add(screening);
            SaveScreenings();
        }

        public List<Account> GetAccounts() => accounts;
        public void AddAccount(Account account)
        {
            accounts.Add(account);
            SaveAccounts();
        }

        public List<Reservation> GetReservations() => reservations;
        public void AddReservation(Reservation reservation)
        {
            reservations.Add(reservation);
            Console.WriteLine($"Reservation {reservation.ReservationId} made.");
            SaveReservations();
        }

        public void RemoveReservation(int reservationId)
        {
            var reservation = reservations
                .FirstOrDefault(r => r.ReservationId == reservationId);

            if (reservation == null)
            {
                Console.WriteLine($"Reservation {reservationId} not found.");
                return;
            }

            reservations.Remove(reservation);

            SaveReservations();

            Console.WriteLine($"Reservation {reservationId} cancelled successfully.");
        }

        public void SaveMovies() => SaveToFile(MoviesFileName, movies);
        public void SaveScreenings() => SaveToFile(ScreeningsFileName, screenings);
        public void SaveAccounts() => SaveToFile(AccountsFileName, accounts);
        public void SaveReservations() => SaveToFile(ReservationsFileName, reservations);

        private List<T> LoadFromFile<T>(string fileName)
        {
            var filePath = Path.Combine(FolderPath, fileName);
            if (File.Exists(filePath))
            {
                try
                {
                    var json = File.ReadAllText(filePath);
                    return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading {fileName}: {ex.Message}");
                }
            }
            return new List<T>();
        }

        private void SaveToFile<T>(string fileName, List<T> data)
        {
            Directory.CreateDirectory(FolderPath);
            var filePath = Path.Combine(FolderPath, fileName);
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
