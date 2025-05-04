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
        void AddReservation(int ticketId, string customerName);
        [OperationContract]
        void CancelReservation(int reservationId);
    }
}
