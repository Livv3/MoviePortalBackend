

namespace movies_BLL.models
{
    public class MovieStreamingsite
    {
        public int MovieStreamingsiteId { get; set; }
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }

        public int StreamingsiteId { get; set; }
        public Streamingsite? Streamingsite { get; set; }
    }
}
