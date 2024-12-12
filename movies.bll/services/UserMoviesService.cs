using Microsoft.AspNetCore.Mvc;
using movies_BLL.IRepositories;
using movies_BLL.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movies_BLL.services
{
    public class UserMoviesService : IUserMoviesService
    {
        private readonly IUserMoviesRepository _repository;


        public UserMoviesService(IUserMoviesRepository repository)
        {
            _repository = repository;
        }

        //GET
        public async Task<IEnumerable<Movie>?> GetWatchlistMoviesForUserAsync(string username)
        {
            IEnumerable<Movie>? watchlist = await _repository.GetWatchlistMoviesForUserAsync(username);
            return watchlist;
        }

        //GET
        public async Task<IEnumerable<Movie>?> GetSeenlistMoviesForUserAsync(string username)
        {
            IEnumerable<Movie>? seenlist = await _repository.GetSeenlistMoviesForUserAsync(username);
            return seenlist;
        }

        //GET
        public async Task<IEnumerable<Movie>?> GetFavouriteMoviesForUserAsync(string username)
        {
            IEnumerable<Movie>? favlist = await _repository.GetFavouriteMoviesForUserAsync(username);
            return favlist;
        }

        //POST
        public async Task<Movie?> AddToWatchListMovieForUserAsync(string username, int movieId)
        {
            return await _repository.AddToWatchListMovieForUserAsync(username, movieId);
        }

        // POST
        public async Task<Movie?> AddSeenMovieForUserAsync(string username, int movieId)
        {
            return await _repository.AddSeenMovieForUserAsync(username, movieId);
        }

        // POST
        public async Task<Movie?> AddFavouriteMovieForUserAsync(string username, int movieId)
        {
            return await _repository.AddFavouriteMovieForUserAsync(username, movieId);
        }

    }
}
