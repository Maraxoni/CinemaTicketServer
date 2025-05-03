using CinemaTicketServer.Classes;
using System.Text.Json;

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
        }

        public List<Movie> GetMovies() => movies.ToList();
        public void AddMovie(Movie movie)
        {
            movies.Add(movie);
            SaveMovies();
        }

        public List<Screening> GetScreenings() => screenings.ToList();
        public void AddScreening(Screening screening)
        {
            screenings.Add(screening);
            SaveScreenings();
        }

        public List<Account> GetAccounts() => accounts.ToList();
        public void AddAccount(Account account)
        {
            accounts.Add(account);
            SaveAccounts();
        }

        public List<Reservation> GetReservations() => reservations.ToList();
        public void AddReservation(Reservation reservation)
        {
            reservations.Add(reservation);
            SaveReservations();
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
