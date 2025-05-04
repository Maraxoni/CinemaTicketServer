using CinemaTicketServer.Classes;
using CoreWCF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemaTicketServer.Services
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class ReservationService : IReservationService
    {
        private readonly IDatabaseService _databaseService;

        public ReservationService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public bool LoginUser()
        {
            Console.WriteLine("Logging in.");
            return true;
        }

        public bool RegisterUser()
        {
            Console.WriteLine("Registering.");
            return true;
        }

        public void AddReservation(int screeningId, string username)
        {
            var user = _databaseService.GetAccounts().FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return;
            }

            int newId = _databaseService.GetReservations().Count > 0
                ? _databaseService.GetReservations().Max(r => r.ReservationId) + 1
                : 1;

            var reservation = new Reservation(newId, screeningId, _databaseService.GetAccounts().IndexOf(user));

            _databaseService.AddReservation(reservation);
            user.AddReservation(reservation.ReservationId);
            _databaseService.SaveAccounts();
            Console.WriteLine($"Reservation {reservation.ReservationId} created for user {username}.");
        }

        public void CancelReservation(int reservationId)
        {
            var reservation = _databaseService.GetReservations().FirstOrDefault(r => r.ReservationId == reservationId);
            if (reservation == null)
            {
                Console.WriteLine("Reservation not found.");
                return;
            }

            var user = _databaseService.GetAccounts().ElementAtOrDefault(reservation.AccountId);
            if (user != null)
            {
                user.ReservationIds.Remove(reservationId);
            }

            _databaseService.GetReservations().Remove(reservation);
            _databaseService.SaveReservations();
            _databaseService.SaveAccounts();

            Console.WriteLine($"Reservation {reservationId} cancelled successfully.");
        }
    }
}
