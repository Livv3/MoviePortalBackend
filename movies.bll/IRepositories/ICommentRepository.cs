using movies_BLL.DTOs;

namespace movies_BLL.IRepositories
{
    public interface ICommentRepository
    {
        // GET: api/Movies/12/comments
        Task<List<CommentViewModel>?> GetCommentsByMovieAsync(int movieId);

        // POST: api/Movies/12/comments
        Task<CommentViewModel> AddCommentAsync(int movieId, CommentViewModel comment);

    }
}
