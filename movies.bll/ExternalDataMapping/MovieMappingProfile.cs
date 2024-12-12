using AutoMapper;
using movies_BLL.models;

namespace movies_BLL.ExternalDataMapping
{
    public class MovieMappingProfile : Profile
    {
        public MovieMappingProfile()
        {
            CreateMap<TmdbMovie, Movie>()
                                        .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                                        .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Overview))
                                        .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Release_Date.Year))
                                        .ForMember(dest => dest.UserRating, opt => opt.MapFrom(src => src.Vote_Average))
                                        .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Poster_Path))
                                        .ForMember(dest => dest.MovieGenres, opt => opt.MapFrom(src => src.Genres != null ? src.Genres : new List<string>()));
        }

    }
}
