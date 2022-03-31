using Movie_API.Models;
using System.Collections.Generic;

namespace Movie_API.Data
{
    public interface IMovieRepo
    {

        IEnumerable<Movie> GetMovieMetadata(int movieId);

        Movie AddMovieMetadata(Movie movie);

        IEnumerable<Stats> GetMovieStats();
    }
}
