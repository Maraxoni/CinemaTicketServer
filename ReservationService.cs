using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaTicketServer
{
    class ReservationService : IReservationService
    {
        public void ReserveTicket(int ticketId, string customerName)
        {
            // Logic to reserve a ticket
            Console.WriteLine($"Ticket {ticketId} reserved for {customerName}");
        }
        public void CancelReservation(int reservationId)
        {
            // Logic to cancel a reservation
            Console.WriteLine($"Reservation {reservationId} cancelled");
        }
    }
}
