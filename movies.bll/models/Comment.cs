using System.Text.Json.Serialization;

namespace movies_BLL.models
{
    public class Comment
    {
        public int Id { get; set; }
        public string? Text { get; set; }

        [JsonIgnore]
        public Movie? Movie { get; set; }
        
        public User? User { get; set; }
    }
}
