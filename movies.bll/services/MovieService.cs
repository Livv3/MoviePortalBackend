using movies_BLL.models;
using movies_BLL.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace movies_BLL.services
{
    public class MovieService: IMovieService
    {
        private readonly IMovieRepository _repository;
        
        
        public MovieService(IMovieRepository repository)
        {
            _repository = repository;
        }

        //Synchronize with tmdb
        public async Task<List<Movie>> GetMoviesFromTmdbAsync()
        {
            return await _repository.GetMoviesFromTmdbAsync();
        }

        //GET all
        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            IEnumerable<Movie> movies = await _repository.GetAllMoviesAsync();
            return movies;
        }

        //GET popular
        public async Task<IEnumerable<Movie>> GetPopularMoviesAsync()
        {
            IEnumerable<Movie> movies = await _repository.GetPopularMoviesAsync();
            return movies;
        }

        //GET by genre
        public async Task<IEnumerable<Movie>> GetMoviesByGenre(string genre)
        {
            IEnumerable<Movie> movies = await _repository.GetMoviesByGenre(genre);
            return movies;
        }

        //GET query
        public async Task<IEnumerable<Movie>> GetMoviesByQueryAsync(string searchQuery)
        {
            IEnumerable<Movie> movies = await _repository.GetMoviesByQueryAsync(searchQuery);
            return movies;
        }


        //GET {id}
        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            var movie = await _repository.GetMovieByIdAsync(id);
            return movie;
        }

        //GET {id} genres
        public async Task<List<Genre>?> GetMovieGenresByIdAsync(int id)
        {
            var genrelist = await _repository.GetMovieGenresByIdAsync(id);
            return genrelist;
        }

        //GET title
        public async Task<Movie?> GetMovieByTitleAsync(string title)
        {
            var movie = await _repository.GetMovieByTitleAsync(title);
            return movie;
        }

        //POST
        public async Task AddMovieAsync(Movie movie)
        {
            await _repository.AddMovieAsync(movie);
        }

        //PUT
        public async Task UpdateMovieAsync(Movie request) 
        {
            await _repository.UpdateMovieAsync(request);
        }

    }
}
