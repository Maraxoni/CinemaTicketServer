using CinemaTicketServer.Classes;
using CoreWCF;
using System.Text.Json;

namespace CinemaTicketServer.Services
{
    [ServiceContract]
    public interface IDatabaseService
    {
        [OperationContract]
        List<Movie> GetMovies();
        [OperationContract]
        void AddMovie(Movie movie);
        [OperationContract]
        void SaveMovies();

        [OperationContract]
        List<Screening> GetScreenings();
        [OperationContract]
        void AddScreening(Screening screening);
        [OperationContract]
        void SaveScreenings();

        [OperationContract]
        List<Account> GetAccounts();
        [OperationContract]
        void AddAccount(Account account);
        [OperationContract]
        void SaveAccounts();

        [OperationContract]
        List<Reservation> GetReservations();
        [OperationContract]
        void AddReservation(Reservation reservation);
        [OperationContract]
        void RemoveReservation(int reservationId);
        [OperationContract]
        void SaveReservations();
    }
}
