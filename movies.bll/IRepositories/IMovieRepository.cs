using Microsoft.AspNetCore.Mvc;
using movies_BLL.models;

namespace movies_BLL.IRepositories
{
    public interface IMovieRepository
    {
        //GET all
        public Task<IEnumerable<Movie>> GetAllMoviesAsync();

        //GET popular
        public Task<IEnumerable<Movie>> GetPopularMoviesAsync();

        //GET by genre
        public Task<IEnumerable<Movie>> GetMoviesByGenre(string genre);

        //GET query
        public Task<IEnumerable<Movie>> GetMoviesByQueryAsync(string searchQuery);

        //GET {id}
        public Task<Movie?> GetMovieByIdAsync(int id);

        //GET {id} genres
        public Task<List<Genre>?> GetMovieGenresByIdAsync(int id);

        //GET title
        public Task<Movie?> GetMovieByTitleAsync(string title);

        //POST
        public Task AddMovieAsync(Movie movie);

        //PUT
        public Task UpdateMovieAsync(Movie request);

        //GET FROM DATABASE
        public Task<List<Movie>> GetMoviesFromTmdbAsync();
        

    }
}
