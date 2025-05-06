using CinemaTicketServer.Classes;
using CoreWCF;
using System.Collections.Generic;

namespace CinemaTicketServer.Services
{
    [ServiceContract]
    public interface IReservationService
    {
        [OperationContract]
        bool LoginUser();
        [OperationContract]
        bool RegisterUser();
        [OperationContract]
        void MakeReservation(int ticketId, string customerName, int[] reservedSeats);
        [OperationContract]
        void CancelReservation(int reservationId);
        [OperationContract]
        void EditReservation(int reservationId, int screeningId, string username, int[] reservedSeats);
    }
}
