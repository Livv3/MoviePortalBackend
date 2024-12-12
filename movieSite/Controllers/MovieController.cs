using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using movies_BLL.models;
using movies_BLL.services;

namespace movies.webapi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _service;

        public MovieController(IMovieService service)
        {
            _service = service;
        }

        //Synchronize with tmdb
        [HttpGet("MoviesFromTmdb")]
        public async Task<ActionResult<List<Movie>>> GetMoviesFromTmdbAsync()
        {
            try
            {
                var movies = await _service.GetMoviesFromTmdbAsync();
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/movies
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        [HttpGet("movies")]
        public async Task<ActionResult<List<Movie>>> GetMovies()
        {
            try
            {
                 var movies = await _service.GetAllMoviesAsync();
                 return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/genre/
        //https://localhost:7233/
        [HttpGet("genre")]
        public async Task<ActionResult<List<Movie>>> GetMoviesByGenre([FromQuery] string genre)
        {
            try
            {
                var movies = await _service.GetMoviesByGenre(genre);
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        // GET: api/Popular
        //https://localhost:7233/
        [HttpGet("Popular")]
        public async Task<ActionResult<List<Movie>>> GetPopular()
        {
            try
            {
                var movies = await _service.GetPopularMoviesAsync();
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/search/query=xxxxxxx
        //https://localhost:7233/
        [HttpGet("search")]
        public async Task<ActionResult<List<Movie>>> GetMoviesByQuery([FromQuery] string query)
        {
            try
            {
                var movies = await _service.GetMoviesByQueryAsync(query);
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        // GET: api/Movies/12
        [HttpGet("Movies/{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            try
            {
                var movie = await _service.GetMovieByIdAsync(id);
                return Ok(movie);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Movies/12/Genres
        [HttpGet("Movies/{movieId}/Genres")]
        public async Task<ActionResult<List<Genre>>> GetMovieGenres(int movieId)
        {
            try
            {
                var genrelist = await _service.GetMovieGenresByIdAsync(movieId);
                return Ok(genrelist);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //GET title
        [HttpGet("MovieTitle")]
        public async Task<ActionResult<Movie>> GetMovieByTitleAsync([FromQuery] string title)
        {
            try
            {
                var movie = await _service.GetMovieByTitleAsync(title);
                return Ok(movie);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //POST movie
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        [HttpPost("Movies")]
        public async Task<ActionResult<List<Movie>>> AddMovie(Movie movie)
        {
            try
            {
                await _service.AddMovieAsync(movie);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //PUT movie
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        [HttpPut("Movies")]
        public async Task<ActionResult<List<Movie>>> UpdateMovie(Movie request)
        {
            try
            {
                await _service.UpdateMovieAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
