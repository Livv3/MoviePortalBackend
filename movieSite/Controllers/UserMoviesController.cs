using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using movies_BLL.models;
using movies_BLL.services;

namespace movies_WebAPI.Controllers
{
    [Route("api/")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles = "User")]
    [ApiController]
    public class UserMoviesController : ControllerBase
    {
        private readonly IUserMoviesService _service;

        public UserMoviesController(IUserMoviesService service)
        {
            _service = service;
        }

        // GET: api/usermovies/watchlist
        [HttpGet("usermovies/watchlist")]
        public async Task<ActionResult<List<Movie>?>> GetWatchlistMoviesForUserAsync(string username)
        {
            var watchlist = await _service.GetWatchlistMoviesForUserAsync(username);
            return Ok(watchlist);
        }

        // GET: api/usermovies/seenlist
        [HttpGet("usermovies/seenlist")]
        public async Task<ActionResult<List<Movie>?>> GetSeenlistMoviesForUserAsync(string username)
        {
            var seenlist = await _service.GetSeenlistMoviesForUserAsync(username);
            return Ok(seenlist);
        }

        // GET: api/usermovies/favourites
        [HttpGet("usermovies/favourites")]
        public async Task<ActionResult<List<Movie>?>> GetFavouriteMoviesForUserAsync(string username)
        {
            var favlist = await _service.GetFavouriteMoviesForUserAsync(username);
            return Ok(favlist);
        }

        
        // POST: api/usermovies/watchlist/add     
        [HttpPost("usermovies/watchlist/add")]
        public async Task<ActionResult<Movie?>> AddToWatchlistMovieForUserAsync([FromQuery] string username, [FromQuery] int movieId)
        {
            var result = await _service.AddToWatchListMovieForUserAsync(username, movieId);
            if (result != null)
            {
                return result;
            }
            else
            {
                return BadRequest("Unable to add the movie to the watchlist.");
            }
        }
        
        // POST: api/usermovies/favourites/add     
        [HttpPost("usermovies/seenlist/add")]
        public async Task<ActionResult<Movie?>> AddSeenMovieForUserAsync([FromQuery] string username, [FromQuery] int movieId)
        {
            var result = await _service.AddSeenMovieForUserAsync(username, movieId);
            if (result != null)
            {
                return result;
            }
            else
            {
                return BadRequest("Unable to add the movie to the watchlist.");
            }
        }

        // POST: api/usermovies/favourites/add     
        [HttpPost("usermovies/favourites/add")]
        public async Task<ActionResult<Movie?>> AddFavouriteMovieForUserAsync([FromQuery] string username, [FromQuery] int movieId)
        {
            var result = await _service.AddFavouriteMovieForUserAsync(username, movieId);
            if (result != null)
            {
                return result;
            }
            else
            {
                return BadRequest("Unable to add the movie to the watchlist.");
            }
        }



    }
}
