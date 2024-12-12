using Microsoft.EntityFrameworkCore;
using movies_BLL.IRepositories;
using movies_BLL.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movies_DAL.Repositories
{
    public class UserMoviesRepository : IUserMoviesRepository
    {

        private readonly DataContext _dataContext;

        public UserMoviesRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        //GET watchlist
        public async Task<IEnumerable<Movie>?> GetWatchlistMoviesForUserAsync(string username)
        {
            string? userId = await _dataContext.Users
                .Where(u => u.UserName == username)
                .Select(u => u.Id)
                .SingleOrDefaultAsync();

            IEnumerable<Movie>? watchlist = new List<Movie>();

            if (userId != null)
            {
                IEnumerable<Movie?>? movies = await _dataContext.UserWatchlistMovies
                    .Where(u => u.UserId == userId)
                    .Select(u => u.Movie)
                    .ToListAsync();
                if (movies != null) watchlist = movies;
            }

            return watchlist;
        }

        //GET seenlist
        public async Task<IEnumerable<Movie>?> GetSeenlistMoviesForUserAsync(string username)
        {
            string? userId = await _dataContext.Users
               .Where(u => u.UserName == username)
               .Select(u => u.Id)
               .SingleOrDefaultAsync();

            IEnumerable<Movie>? seenlist = new List<Movie>();

            if (userId != null)
            {
                IEnumerable<Movie?>? movies = await _dataContext.UserSeenMovies
                    .Where(u => u.UserId == userId)
                    .Select(u => u.Movie)
                    .ToListAsync();
                if (movies != null) seenlist = movies;
            }

            return seenlist;
        }

        //GET favouritelist
        public async Task<IEnumerable<Movie>?> GetFavouriteMoviesForUserAsync(string username)
        {
            string? userId = await _dataContext.Users
                .Where(u => u.UserName == username)
                .Select(u => u.Id)
                .SingleOrDefaultAsync();

            IEnumerable<Movie>? favlist = new List<Movie>();

            if (userId != null)
            {
                IEnumerable<Movie?>? movies = await _dataContext.UserFavouriteMovies
                    .Where(u => u.UserId == userId)
                    .Select(u => u.Movie)
                    .ToListAsync();
                if (movies != null) favlist = movies;
            }

            return favlist;
        }

        //POST
        public async Task<Movie?> AddToWatchListMovieForUserAsync(string username, int movieId)
        {
            //user id megszerez
            User? user = await _dataContext.Users
                .Where(u => u.UserName == username)
                .SingleOrDefaultAsync();

            //film megszerez
            Movie? movie = await _dataContext.Movies
                .Where(m => m.Id == movieId)
                .SingleOrDefaultAsync();

            if (user != null)
            {
            
                //van már ez a kapcsolat?
                UserWatchlistMovie? existingUserMovie = null;
                existingUserMovie = _dataContext.UserWatchlistMovies.FirstOrDefault(wm => wm.MovieId == movieId && wm.UserId == user.Id);
                
                //ha még nincs megcsinál
                if(existingUserMovie == null && movie != null)
                {
                    var newUserMovie = new UserWatchlistMovie
                    {
                        MovieId = movieId,
                        UserId = user.Id,
                        User = user, 
                        Movie = movie
                    };

                    if (user.WatchlistedMovies == null) 
                    { 
                        user.WatchlistedMovies = new List<UserWatchlistMovie>();
                    }
                    user.WatchlistedMovies.Add(newUserMovie);

                    if (movie.WatchlistedByUsers == null)
                    {
                        movie.WatchlistedByUsers = new List<UserWatchlistMovie>();
                    }
                    movie.WatchlistedByUsers.Add(newUserMovie);

                }
              
                await _dataContext.SaveChangesAsync();
                return movie;
            }

            return new Movie();

        }

        //POST
        public async Task<Movie?> AddSeenMovieForUserAsync(string username, int movieId)
        {
            //user id megszerez
            User? user = await _dataContext.Users
                .Where(u => u.UserName == username)
                .SingleOrDefaultAsync();

            //film megszerez
            Movie? movie = await _dataContext.Movies
                .Where(m => m.Id == movieId)
                .SingleOrDefaultAsync();

            if (user != null)
            {

                //van már ez a kapcsolat?
                UserSeenMovie? existingUserMovie = null;
                existingUserMovie = _dataContext.UserSeenMovies.FirstOrDefault(wm => wm.MovieId == movieId && wm.UserId == user.Id);

                //ha még nincs megcsinál
                if (existingUserMovie == null && movie != null)
                {
                    var newUserMovie = new UserSeenMovie
                    {
                        MovieId = movieId,
                        UserId = user.Id,
                        User = user,
                        Movie = movie
                    };

                    if (user.SeenMovies == null)
                    {
                        user.SeenMovies = new List<UserSeenMovie>();
                    }
                    user.SeenMovies.Add(newUserMovie);

                    if (movie.SeenByUsers == null)
                    {
                        movie.SeenByUsers = new List<UserSeenMovie>();
                    }
                    movie.SeenByUsers.Add(newUserMovie);

                }

                await _dataContext.SaveChangesAsync();
                return movie;
            }

            return new Movie();
        }

        //POST
        public async Task<Movie?> AddFavouriteMovieForUserAsync(string username, int movieId)
        {
            //user id megszerez
            User? user = await _dataContext.Users
                .Where(u => u.UserName == username)
                .SingleOrDefaultAsync();

            //film megszerez
            Movie? movie = await _dataContext.Movies
                .Where(m => m.Id == movieId)
                .SingleOrDefaultAsync();

            if (user != null)
            {

                //van már ez a kapcsolat?
                UserFavouriteMovie? existingUserMovie = null;
                existingUserMovie = _dataContext.UserFavouriteMovies.FirstOrDefault(wm => wm.MovieId == movieId && wm.UserId == user.Id);

                //ha még nincs megcsinál
                if (existingUserMovie == null && movie != null)
                {
                    var newUserMovie = new UserFavouriteMovie
                    {
                        MovieId = movieId,
                        UserId = user.Id,
                        User = user,
                        Movie = movie
                    };

                    if (user.FavouriteMovies == null)
                    {
                        user.FavouriteMovies = new List<UserFavouriteMovie>();
                    }
                    user.FavouriteMovies.Add(newUserMovie);

                    if (movie.FavouritedByUsers == null)
                    {
                        movie.FavouritedByUsers = new List<UserFavouriteMovie>();
                    }
                    movie.FavouritedByUsers.Add(newUserMovie);

                }

                await _dataContext.SaveChangesAsync();
                return movie;
            }

            return new Movie();
        }

    }
}
