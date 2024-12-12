using System.Text.Json.Serialization;

namespace movies_BLL.models
{
    public class Movie
    {
        public int Id { get; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int Year { get; set; }
        public int UserRating { get; set; }
        public string? Url { get; set; } 
        public string? TrailerUrl { get; set; }
        public int? Runtime { get; set; }
        public string? Tagline { get; set; }

        [JsonIgnore]
        public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();

        [JsonIgnore]
        public ICollection<MovieStreamingsite> MovieStreamingsites { get; set; } = new List<MovieStreamingsite>();

        [JsonIgnore]
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        [JsonIgnore]
        public ICollection<UserFavouriteMovie>? FavouritedByUsers { get; set; }
        [JsonIgnore]
        public ICollection<UserSeenMovie>? SeenByUsers { get; set; }
        [JsonIgnore]
        public ICollection<UserWatchlistMovie>? WatchlistedByUsers { get; set; }

    }
}