using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using movies_BLL.DTOs;
using movies_BLL.services;

namespace movies_WebAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _service;

        public CommentController(ICommentService service)
        {
            _service = service;
        }


        // GET: api/Movies/12/comments
        [HttpGet("Movies/{movieId}/comments")]
        public async Task<ActionResult<List<CommentViewModel>?>> GetCommentsByMovie(int movieId)
        {
            try
            {
                var comments = await _service.GetCommentsByMovieAsync(movieId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        // POST: api/Movies/12/comments
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "User")]
        [HttpPost("Movies/{movieId}/comments")]
        public async Task<ActionResult<CommentViewModel>> AddComment(int movieId, CommentViewModel comment)
        {
            try
            {
                string username = HttpContext.User.Identity.Name;
                comment.Username = username;
                await _service.AddCommentAsync(movieId, comment);
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
