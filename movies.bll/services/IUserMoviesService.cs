using Microsoft.AspNetCore.Mvc;
using movies_BLL.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movies_BLL.services
{
    public interface IUserMoviesService
    {

        //GET
        public Task<IEnumerable<Movie>?> GetWatchlistMoviesForUserAsync(string username);
        public Task<IEnumerable<Movie>?> GetSeenlistMoviesForUserAsync(string username);
        public Task<IEnumerable<Movie>?> GetFavouriteMoviesForUserAsync(string username);


        //POST
        public Task<Movie?> AddToWatchListMovieForUserAsync(string username, int movieId);
        public Task<Movie?> AddSeenMovieForUserAsync(string username, int movieId);
        public Task<Movie?> AddFavouriteMovieForUserAsync(string username, int movieId);

    }
}
