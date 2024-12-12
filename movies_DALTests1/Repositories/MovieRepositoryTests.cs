using AutoMapper;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using movies_BLL.models;
using movies_DAL.Repositories;
using Xunit;
using FluentAssertions;
using movies_DAL;

namespace movies_DALTests1.Repositories
{
    public class MovieRepositoryTests
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        private async Task<DataContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new DataContext(options);
            databaseContext.Database.EnsureCreated();

            if (await databaseContext.Movies.CountAsync() <= 0)
            {
                Genre actionGenre = new Genre { Name = "Action" };
                databaseContext.Genres.Add(actionGenre);

                for (int i = 1; i <= 20; i++)
                {
                    databaseContext.Movies.Add(
                        new Movie
                        {
                            Title = $"Movie {i}",
                            Description = $"Description for Movie {i}",
                            Year = 2000 + i,
                            UserRating = 7 + i % 3,
                            Url = $"https://example.com/movie{i}_poster.jpg",
                            TrailerUrl = $"https://example.com/movie{i}_trailer.mp4",
                            Runtime = 120 + i,
                            Tagline = $"Tagline for Movie {i}",
                            MovieGenres = i == 5 ? new List<MovieGenre> { new MovieGenre { Genre = actionGenre } } : null
                        }
                         
                    );
                }

                await databaseContext.SaveChangesAsync();
            }
            return databaseContext;
        }


        [Fact]
        public async void MovieRepository_GetMovie_ReturnMovie()
        {
            var title = "Movie 3";
            var tagline = "Tagline for Movie 3";
            var runtime = 123;
            var dbContext = await GetDatabaseContext();
            var movieRepository = new MovieRepository(dbContext, _httpClient, _mapper);

            Movie? result = await movieRepository.GetMovieByIdAsync(3);

            result.Should().NotBeNull();
            result.Title.Should().Be(title);
            result.Tagline.Should().Be(tagline);
            result.Runtime.Should().Be(runtime);
        }

        [Fact]
        public async void MovieRepository_GetPopular_ReturnPopular()
        {
            var dbContext = await GetDatabaseContext();
            var movieRepository = new MovieRepository(dbContext, _httpClient, _mapper);

            IEnumerable<Movie>? result = await movieRepository.GetPopularMoviesAsync();

            result.Should().NotBeNull();
            result.Count().Should().Be(20);
        }

        [Fact]
        public async void MovieRepository_GetMoviesByGenre_ReturnMoviesByGenre()
        {
            
            string expectedGenreName = "Action";
            var dbContext = await GetDatabaseContext();
            var movieRepository = new MovieRepository(dbContext, _httpClient, _mapper);

            IEnumerable<Genre>? result = await movieRepository.GetMovieGenresByIdAsync(5);

            result.Should().NotBeNull();
            result.Select(g => g.Name).Should().Contain(expectedGenreName);
        }


    }
}
