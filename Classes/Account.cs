using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CinemaTicketServer.Classes
{
    public enum AccountType
    {
        User,
        Admin
    }
    [DataContract]
    public class Account
    {
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public AccountType Type { get; set; }
        [DataMember]
        public HashSet<int> ReservationIds { get; set; }

        public Account(string username, string password, AccountType type)
        {
            Username = username;
            Password = password;
            Type = type;
            ReservationIds = new HashSet<int>();
        }

        public void AddReservation(int reservationId)
        {
            ReservationIds.Add(reservationId);
        }

        public bool HasReservation(int reservationId)
        {
            return ReservationIds.Contains(reservationId);
        }

        public bool IsAdmin()
        {
            return Type == AccountType.Admin;
        }

        public bool IsUser()
        {
            return Type == AccountType.User;
        }
    }
}
