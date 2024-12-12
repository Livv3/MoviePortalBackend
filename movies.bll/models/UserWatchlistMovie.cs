

namespace movies_BLL.models
{
    public class UserWatchlistMovie
    {
        public string? UserId { get; set; }
        public User? User { get; set; }

        public int? MovieId { get; set; }
        public Movie? Movie { get; set; }
    }

}
