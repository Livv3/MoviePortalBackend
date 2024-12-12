
using Microsoft.AspNetCore.Mvc;
using movies_BLL.models;

namespace movies_BLL.IRepositories
{
    public interface IStreamingsiteRepository
    {
        //GET
        public Task<List<Streamingsite>?> GetStreamingsitesForMovieAsync(int movieId);

        //POST
        public Task<List<Streamingsite>> AddStreamingsitesForMovieAsync(int movieId, List<Streamingsite> sites);
    }
}
