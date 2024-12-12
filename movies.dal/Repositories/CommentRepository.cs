using Microsoft.EntityFrameworkCore;
using movies_BLL.DTOs;
using movies_BLL.models;
using movies_BLL.IRepositories;

namespace movies_DAL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _dataContext;

        public CommentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        //GET
        public async Task<List<CommentViewModel>?> GetCommentsByMovieAsync(int movieId)
        {
            var movie = await _dataContext.Movies
                .Include(m => m.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                return null;
            }

            var comments = movie.Comments
                .Select(c => new CommentViewModel
                {
                    Username = c.User?.UserName,
                    Text = c.Text
                })
                .ToList();

            return comments;
        }



        //POST
        public async Task<CommentViewModel> AddCommentAsync(int movieId, CommentViewModel comment)
        {
            if (comment.Text != null && ContainsHarmfulCharacters(comment.Text))
            {
                throw new InvalidOperationException("Might contain harmful text.");
            }

            var movie = await _dataContext.Movies.FindAsync(movieId) ?? throw new ArgumentException("Invalid movie ID.");
            var user = await _dataContext.Users.SingleOrDefaultAsync(u => u.UserName == comment.Username) ?? throw new ArgumentException("User with the provided username does not exist.");
            var newComment = new Comment
            {
                Text = comment.Text
            };

            newComment.User = user;
            newComment.Movie = movie;


            movie.Comments.Add(newComment);

            await _dataContext.SaveChangesAsync();

            return comment;
        }

        private bool ContainsHarmfulCharacters(string input)
        {
            return input.Contains(";") || input.Contains("--") || input.ToUpper().Contains("DROP") || input.ToUpper().Contains("DELETE") || input.ToUpper().Contains("UPDATE");
        }

    }

}
