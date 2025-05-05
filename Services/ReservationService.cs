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

        public void MakeReservation(int screeningId, string username, int[] reservedSeats)
        {
            Console.WriteLine(username);
            var user = _databaseService.GetAccounts().FirstOrDefault(u => u.Username == username);   
            Console.WriteLine(_databaseService.GetAccounts());
            Console.WriteLine(user);

            if (user == null)
            {
                Console.WriteLine("User not found. ");
                return;
            }

            int newId = _databaseService.GetReservations().Count > 0
                ? _databaseService.GetReservations().Max(r => r.ReservationId) + 1
                : 1;

            var reservation = new Reservation(newId, screeningId, username, reservedSeats);

            var screening = _databaseService.GetScreenings()
                                    .FirstOrDefault(s => s.ScreeningID == screeningId);

            foreach (var seatIndex in reservedSeats)
            {
                if (seatIndex < 0 || seatIndex >= screening.AvailableSeats.Length)
                    throw new FaultException($"Invalid seat index: {seatIndex}");

                if (!screening.AvailableSeats[seatIndex])
                    throw new FaultException($"Seat {seatIndex + 1} is already reserved.");

                screening.AvailableSeats[seatIndex] = false;
            }

            _databaseService.AddReservation(reservation);
            _databaseService.SaveReservations();
            _databaseService.SaveScreenings();
            Console.WriteLine($"Reservation {reservation.ReservationId} created for user {username}.");
        }

        public void CancelReservation(int reservationId)
        {
            var reservation = _databaseService
                .GetReservations()
                .FirstOrDefault(r => r.ReservationId == reservationId);

            if (reservation == null)
            {
                Console.WriteLine($"Reservation {reservationId} not found.");
                return;
            }

            var user = _databaseService
                .GetAccounts()
                .FirstOrDefault(u => u.Username == reservation.AccountUsername);

            if (user != null)
            {
                _databaseService.RemoveReservation(reservationId);
                _databaseService.SaveAccounts();
            }
            else
            {
                Console.WriteLine($"User '{reservation.AccountUsername}' not found.");
            }

            var screening = _databaseService.GetScreenings()
                                    .FirstOrDefault(s => s.ScreeningID == reservation.ScreeningId);
            Console.WriteLine("Screening Check: " + screening);
            if (screening != null)
            {
                foreach (var seatIndex in reservation.ReservedSeats)
                {
                    if (seatIndex >= 0 && seatIndex < screening.AvailableSeats.Length)
                    {
                        screening.AvailableSeats[seatIndex] = true;
                    }
                }
                _databaseService.SaveScreenings();
            }

            Console.WriteLine($"Reservation {reservationId} cancelled successfully.");
        }
    }
}
