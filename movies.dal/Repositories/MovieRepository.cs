using AutoMapper;
using Microsoft.EntityFrameworkCore;
using movies_BLL.ExternalDataMapping;
using movies_BLL.models;
using movies_BLL.IRepositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace movies_DAL.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly DataContext _context;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly string apiKey = "846c016387cccc283dfb523311f39ca1";

        public MovieRepository(DataContext context, HttpClient httpClient, IMapper mapper)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MovieMappingProfile>();

            });

            _mapper = config.CreateMapper();
            _context = context;
            _httpClient = httpClient;
            _mapper = mapper;
        }



        //with the help of httpClient, movies from external api
        public async Task<List<Movie>> GetMoviesFromTmdbAsync()
        {
            var tmdbUrl = $"https://api.themoviedb.org/3/movie/popular?api_key=846c016387cccc283dfb523311f39ca1&page=";

            List<Movie> moviesToAdd = new();
            List<TmdbMovie> allTmdbMovies = new();

            int pagesToFetch = 25;

            for (int i = 1; i <= pagesToFetch; i++)
            {
                var response = await _httpClient.GetAsync(tmdbUrl + i);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var tmdbMovies = JsonConvert.DeserializeObject<TmdbMovieApiResponse>(content);

                if (tmdbMovies?.Results != null)
                {
                    allTmdbMovies.AddRange(tmdbMovies.Results);
                }
            }


            var genreListUrl = $"https://api.themoviedb.org/3/genre/movie/list?api_key=846c016387cccc283dfb523311f39ca1";
            var genreListResponse = await _httpClient.GetAsync(genreListUrl);
            genreListResponse.EnsureSuccessStatusCode();
            var genreListContent = await genreListResponse.Content.ReadAsStringAsync();
            var tmdbGenres = JsonConvert.DeserializeObject<TmdbGenreList>(genreListContent);

           
#pragma warning disable CS8714
            var genreNameToEntityMap = _context?.Genres?.ToDictionary(g => g.Name, g => g);
#pragma warning restore CS8714

            var genreIdToNameMapping = tmdbGenres?.Genres?.ToDictionary(g => g.Id, g => g.Name);


            foreach (var tmdbMovie in allTmdbMovies)
            {
                Movie? existingMovie = null;

                // Check if a movie with the same title already exists in the database
                if (tmdbMovie.Title != null)
                {
                    existingMovie = await GetMovieByTitleAsync(tmdbMovie.Title);
                }


                if (existingMovie == null)
                {
                    // Retrieve the trailer key for the movie
                    var videosUrl = $"https://api.themoviedb.org/3/movie/{tmdbMovie.Id}/videos?api_key=846c016387cccc283dfb523311f39ca1";
                    var videosResponse = await _httpClient.GetAsync(videosUrl);
                    videosResponse.EnsureSuccessStatusCode();
                    var videosContent = await videosResponse.Content.ReadAsStringAsync();
                    TmdbVideoApiResponse? tmdbVideos = JsonConvert.DeserializeObject<TmdbVideoApiResponse>(videosContent);
                    var trailer = tmdbVideos?.Results?.FirstOrDefault(v => v.Type == "Trailer");

                    // Map the movie object and set the trailer URL
                    var movie = _mapper.Map<Movie>(tmdbMovie);
                    if (trailer != null)
                    {
                        movie.TrailerUrl = trailer.Key;
                    }

                    await GetTaglineRuntimeFromTmdb(tmdbMovie.Id, movie);


                    // Fetch and associate Genres with the movie
                    if (tmdbMovie.Genre_Ids != null)
                    {
                        foreach (var genreId in tmdbMovie.Genre_Ids)
                        {
                            if (genreIdToNameMapping != null && genreNameToEntityMap != null)
                            {
                                if (genreIdToNameMapping.ContainsKey(genreId))
                                {
                                    var genreName = genreIdToNameMapping[genreId];

                                    if (genreNameToEntityMap.TryGetValue(genreName, out var existingGenre))
                                    {
                                        var movieGenre = new MovieGenre
                                        {
                                            Movie = movie,
                                            Genre = existingGenre
                                        };

                                        _context?.MovieGenres.Add(movieGenre);
                                    }
                                    else
                                    {
                                        // Create a new genre if it doesn't exist
                                        var newGenre = new Genre { Name = genreName };
                                        genreNameToEntityMap[genreName] = newGenre;

                                        // Create a MovieGenre entry for the new genre
                                        var movieGenre = new MovieGenre
                                        {
                                            Movie = movie,
                                            Genre = newGenre
                                        };

                                        _context?.MovieGenres.Add(movieGenre);
                                    }
                                }
                            }
                        }
                    }
                    moviesToAdd.Add(movie);
                }
            }

            foreach (var movieToAdd in moviesToAdd)
            {
                await AddMovieAsync(movieToAdd);
            }

            return moviesToAdd;
        }

        private async Task GetTaglineRuntimeFromTmdb(int tmdbId, Movie movie)
        {
            try
            {
                string apiUrl = $"https://api.themoviedb.org/3/movie/{tmdbId}?api_key={apiKey}&language=en-US";

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var movieInfo = JObject.Parse(jsonResponse);

                    if (movieInfo["tagline"] != null)
                    {
                        movie.Tagline = movieInfo["tagline"]?.ToString();
                    }
                    if (movieInfo["runtime"] != null)
                    {
                        movie.Runtime = (int)movieInfo["runtime"];
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.ReasonPhrase}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Exception: {ex.Message}");
            }

        }


        //GET all
        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            var movies = from m in _context.Movies
                         select m;
            return await movies.ToListAsync();
        }

        //GET popular
        public async Task<IEnumerable<Movie>> GetPopularMoviesAsync()
        {
            var movies = _context.Movies
                  .Include(m => m.Comments)
                  .Include(m => m.MovieGenres)
                  .ThenInclude(mg => mg.Genre)
                  .OrderBy(m => m.Id)
                  .Take(20);
            return await movies.ToListAsync();
        }

        //GET by genre
        public async Task<IEnumerable<Movie>> GetMoviesByGenre(string genre)
        {
            var movies = _context.Movies
                                        .Join(
                                            _context.MovieGenres,
                                            movie => movie.Id,
                                            movieGenre => movieGenre.MovieId,
                                            (movie, movieGenre) => new { movie, movieGenre })
                                        .Join(
                                            _context.Genres,
                                            movieGenre => movieGenre.movieGenre.GenreId,
                                            genre => genre.Id,
                                            (movieGenre, genre) => new { movieGenre, genre })
                                        .Where(joinedData => joinedData.genre.Name == genre)
                                        .Select(joinedData => joinedData.movieGenre.movie);

            return await movies.ToListAsync();
        }

        // GET searchquery
        public async Task<IEnumerable<Movie>> GetMoviesByQueryAsync(string searchQuery)
        {
            var movies = _context.Movies
                .Where(m => m.Title.Contains(searchQuery))
                .OrderBy(m => m.Id);
            return await movies.ToListAsync();
        }


        //GET {id}
        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            var movie = await _context.Movies
                   .Include(m => m.Comments)
                   .Include(m => m.MovieGenres)
                   .ThenInclude(mg => mg.Genre)
                   .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) return null;
            return movie;
        }

        //GET {id} /genre
        public async Task<List<Genre>?> GetMovieGenresByIdAsync(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return null;
            var genres = movie.MovieGenres
                                        .Select(mg => mg.Genre)
                                        .Where(g => g != null) 
                                        .Select(g => g!)    
                                        .ToList();
            return genres;
        }

        //GET title
        public async Task<Movie?> GetMovieByTitleAsync(string title)
        {
            var existingMovie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Title == title);

            return existingMovie;
        }


        //POST
        public async Task AddMovieAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
        }

        //PUT
        public async Task UpdateMovieAsync(Movie request)
        {
            var dbMovie = await _context.Movies.FindAsync(request.Id);

            if (dbMovie == null) return;

            dbMovie.Title = request.Title;
            dbMovie.Description = request.Description;
            dbMovie.Year = request.Year;
            dbMovie.Url = request.Url;
            dbMovie.Comments = request.Comments;

            await _context.SaveChangesAsync();
        }

    }

}