

namespace movies_BLL.ExternalDataMapping
{
    public class TmdbVideoApiResponse
    {
        public List<TmdbVideo>? Results { get; set; }
    }

    public class TmdbVideo
    {
        public string? Key { get; set; }
        public string? Name { get; set; }
        public string? Site { get; set; }
        public int? Size { get; set; }
        public string? Type { get; set; }
    }

}
