using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Movie_API.Data;
using Movie_API.Dtos;
using Movie_API.Models;
using System.Collections.Generic;

namespace Movie_API.Controllers
{
    [Route("")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepo _repository;
        private readonly IMapper _mapper;

        public MoviesController(IMovieRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        [HttpPost("metadata")]
        public IActionResult PostMetaData([FromBody] MovieMetaDataCreateDto metadata)
        {
            var movie = _mapper.Map<Movie>(metadata);
            _repository.AddMovieMetadata(movie);

            var movieToReturn = _mapper.Map<MovieMetaDataReadDto>(movie);

            return CreatedAtRoute(nameof(GetMetadatabyId), new { movieId = movie.movieId }, movieToReturn);

        }

        [HttpGet("metadata/{movieId}", Name = "GetMetadatabyId")]
        public ActionResult<IEnumerable<MovieStatsReadDto>> GetMetadatabyId(int movieId)
        {

            var moviemetadata = _repository.GetMovieMetadata(movieId);
            //throw 404

            return Ok(_mapper.Map<IEnumerable<MovieMetaDataReadDto>>(moviemetadata));
        }



        [HttpGet("stats")]
        public ActionResult<IEnumerable<MovieStatsReadDto>> GetMovieStats()
        {

            var moviestats = _repository.GetMovieStats();



            return Ok(_mapper.Map<IEnumerable<MovieStatsReadDto>>(moviestats));
        }
    }
}
