using AutoMapper;
using Movie_API.Dtos;
using Movie_API.Models;

namespace Movie_API.Profiles
{
    public class MoviesProfile : Profile
    {
        public MoviesProfile()
        {
            CreateMap<Movie, MovieMetaDataReadDto>();
            CreateMap<MovieMetaDataCreateDto, Movie>();
            CreateMap<Stats, MovieStatsReadDto>();
        }

    }
}
