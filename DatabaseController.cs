using CinemaTicketServer.Services;
using CinemaTicketServer.Classes; // Ensure your Movie, Reservation, and Screening classes are correctly referenced
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketServer
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IDatabaseService _databaseService;

        public MoviesController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpGet("movies")]
        public ActionResult<List<Movie>> GetMovies()
        {
            var movies = _databaseService.GetMovies();  // Fetch movies from the service
            return Ok(movies);  // Return the list of movies as JSON
        }

        [HttpGet("reservations")]
        public ActionResult<List<Reservation>> GetReservations()
        {
            var reservations = _databaseService.GetReservations();  // Fetch reservations from the service
            return Ok(reservations);  // Return the list of reservations as JSON
        }

        // Endpoint to get the list of screenings
        [HttpGet("screenings")]
        public ActionResult<List<Screening>> GetScreenings()
        {
            var screenings = _databaseService.GetScreenings();  // Fetch screenings from the service
            return Ok(screenings);  // Return the list of screenings as JSON
        }
    }
}
