using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using movies_BLL.IRepositories;
using movies_BLL.models;

namespace movies_DAL.Repositories
{
    public class StreamingsiteRepository : IStreamingsiteRepository
    {
        private readonly DataContext _dataContext;

        public StreamingsiteRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        //GET
        public async Task<List<Streamingsite>?> GetStreamingsitesForMovieAsync(int movieId)
        {
            var sites = await _dataContext.MovieStreamingsites
                .Where(ms => ms.MovieId == movieId)
                .Select(ms => ms.Streamingsite)
                .ToListAsync();

            return sites;

        }

        //POST
        public async Task<List<Streamingsite>> AddStreamingsitesForMovieAsync(int movieId, List<Streamingsite> sites)
        {
            var movie = await _dataContext.Movies.Include(m => m.MovieStreamingsites).FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie != null)
            {
                foreach (var site in sites)
                {
                    if (site.Name != null && ContainsHarmfulCharacters(site.Name))
                    {
                        throw new InvalidOperationException("Site name might contain harmful elements");
                    }

                    var existingSite = await _dataContext.Streamingsites.FirstOrDefaultAsync(s => s.Name == site.Name);
                    MovieStreamingsite? existingMovieStreamingsite = null;


                    if (existingSite != null)
                    {
                        existingMovieStreamingsite = _dataContext.MovieStreamingsites.FirstOrDefault(ms =>
                        ms.MovieId == movieId && ms.StreamingsiteId == existingSite.Id);

                    }
                    if (existingMovieStreamingsite == null)
                    {
                        if (existingSite == null)
                        {
                            existingSite = new Streamingsite
                            {
                                Name = site.Name
                            };
                            _dataContext.Streamingsites.Add(existingSite);
                        }

                        var movieStreamingsite = new MovieStreamingsite
                        {
                            MovieId = movieId,
                            Streamingsite = existingSite
                        };

                        movie.MovieStreamingsites.Add(movieStreamingsite);
                    }
                   
                }

                await _dataContext.SaveChangesAsync();
                return sites;
            
            }

            return new List<Streamingsite>();
        }


        private bool ContainsHarmfulCharacters(string input)
        {
            return input.Contains(";") || input.Contains("--") || input.ToUpper().Contains("DROP") || input.ToUpper().Contains("DELETE") || input.ToUpper().Contains("UPDATE");
        }

    }
}
