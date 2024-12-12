using Microsoft.AspNetCore.Identity;


namespace movies_BLL.models
{
    public class User : IdentityUser<string>
    {
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<UserFavouriteMovie>? FavouriteMovies { get; set; }
        public ICollection<UserSeenMovie>? SeenMovies { get; set; }
        public ICollection<UserWatchlistMovie>? WatchlistedMovies { get; set; }


    }
}
