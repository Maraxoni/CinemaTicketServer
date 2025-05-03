using CinemaTicketServer.Classes;
using CoreWCF;
using System.Collections.Generic;

namespace CinemaTicketServer.Services
{
    [ServiceContract]
    public interface IReservationService
    {
        [OperationContract]
        void AddUser(string username, string password, AccountType accountType);
        [OperationContract]
        void AddReservation(int ticketId, string customerName);
        [OperationContract]
        void CancelReservation(int reservationId);
    }
}
