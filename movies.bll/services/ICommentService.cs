using movies_BLL.DTOs;
using movies_BLL.models;

namespace movies_BLL.services
{
    public interface ICommentService
    {
        // GET: api/Movies/12/comments
        Task<List<CommentViewModel>?> GetCommentsByMovieAsync(int movieId);

        // POST: api/Movies/12/comments
        Task<CommentViewModel> AddCommentAsync(int movieId, CommentViewModel comment);
      

    }
}
