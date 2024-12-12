using Microsoft.AspNetCore.Mvc;
using movies_BLL.DTOs;
using movies_BLL.IRepositories;
using movies_BLL.models;

namespace movies_BLL.services
{
    public class StreamingsiteService : IStreamingsiteService
    {
        private readonly IStreamingsiteRepository _repository;

        public StreamingsiteService(IStreamingsiteRepository repository)
        {
            _repository = repository;
        }

        //GET
        public async Task<List<Streamingsite>?> GetStreamingsitesForMovieAsync(int movieId)
        {
            List<Streamingsite>? sites = await _repository.GetStreamingsitesForMovieAsync(movieId);
            return sites;
        }

        //POST
        public async Task<ActionResult<List<Streamingsite>>> AddStreamingsitesForMovieAsync(int movieId, List<Streamingsite> sites)
        {
            await _repository.AddStreamingsitesForMovieAsync(movieId, sites);
            return sites;
        }

    }
}
