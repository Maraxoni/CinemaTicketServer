using System;
using System.Runtime.Serialization;

namespace CinemaTicketServer.Classes
{
    [DataContract]
    public class Reservation
    {
        [DataMember]
        public int ReservationId { get; set; }
        [DataMember]
        public int ScreeningId { get; set; }
        [DataMember]
        public int AccountId { get; set; }

        public Reservation(int reservationId, int screeningId, int accountId)
        {
            ReservationId = reservationId;
            ScreeningId = screeningId;
            AccountId = accountId;
        }
    }
}
