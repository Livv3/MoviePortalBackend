using movies_BLL.DTOs;
using movies_BLL.IRepositories;

namespace movies_BLL.services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repository;


        public CommentService(ICommentRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Movies/12/comments
        public async Task<List<CommentViewModel>?> GetCommentsByMovieAsync(int movieId)
        {
            List<CommentViewModel>? comments = await _repository.GetCommentsByMovieAsync(movieId);
            return comments;
        }

        // POST: api/Movies/12/comments
        public async Task<CommentViewModel> AddCommentAsync(int movieId, CommentViewModel comment)
        {
            await _repository.AddCommentAsync(movieId, comment);
            return comment;
        }


    }
}
