

using Microsoft.AspNetCore.Mvc;
using movies_BLL.models;

namespace movies_BLL.services
{
    public interface IStreamingsiteService
    {
        //GET
        public Task<List<Streamingsite>?> GetStreamingsitesForMovieAsync(int movieId);

        //POST
        public Task<ActionResult<List<Streamingsite>>> AddStreamingsitesForMovieAsync(int movieId, List<Streamingsite> sites);

    }
}
