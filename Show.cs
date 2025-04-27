namespace CinemaTicketServer
{
    public class Show
    {
        public Movie Movie { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public bool[] AvailableSeats { get; private set; }

        public Show(Movie movie, DateTime startTime, TimeSpan duration, int seatCount)
        {
            Movie = movie;
            StartTime = startTime;
            EndTime = startTime.Add(duration);
            AvailableSeats = new bool[seatCount];

            for (int i = 0; i < AvailableSeats.Length; i++)
            {
                AvailableSeats[i] = true;
            }
        }
    }
}
