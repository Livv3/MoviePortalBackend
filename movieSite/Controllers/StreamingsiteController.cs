using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using movies_BLL.DTOs;
using movies_BLL.models;
using movies_BLL.services;

namespace movies_WebAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class StreamingsiteController : ControllerBase
    {
        private readonly IStreamingsiteService _service;

        public StreamingsiteController(IStreamingsiteService service)
        {
            _service = service;
        }

        // GET: api/Movies/12/streamingsites
        [HttpGet("Movies/{movieId}/streamingsites")]
        public async Task<ActionResult<List<Streamingsite>?>> GetStreamingsitesForMovieAsync(int movieId)
        {
            var sites = await _service.GetStreamingsitesForMovieAsync(movieId);
            return Ok(sites);
        }

        // POST: api/Movies/12/streamingsites
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        [HttpPost("Movies/{movieId}/streamingsites")]
        public async Task<ActionResult<List<Streamingsite>>> AddStreamingsitesForMovieAsync(int movieId, List<Streamingsite> sites)
        {
            await _service.AddStreamingsitesForMovieAsync(movieId, sites);
            return Ok(sites);
        }
    }
}
