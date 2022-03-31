using IronXL;
using Movie_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace Movie_API.Data
{
    public class MovieRepo : IMovieRepo
    {
        public static string metadataCSVlocation = @"C:\Users\Administrator\Downloads\movie_api\metadata.csv";
        public static string statsCSVlocation = @"C:\Users\Administrator\Downloads\movie_api\stats.csv";

        public List<Movie> Database = new List<Movie>();
        public Movie AddMovieMetadata(Movie movie)
        {

            Database.Add(movie);
            return movie;
        }

        public IEnumerable<Movie> GetMovieMetadata(int movieId)
        {
            List<Movie> AllMovies = GetAllMovies();

            AllMovies = AllMovies.Where(m => m.movieId == movieId).ToList();

            var movieswithhighestid =
                AllMovies
                    .GroupBy(a => a.language)
                    .Select(g => g.OrderByDescending(a => a.Id).First())
                    .ToList();

            return movieswithhighestid.OrderBy(x => x.language);

        }

        private static List<Movie> GetAllMovies()
        {
            WorkBook workbook = WorkBook.LoadCSV(metadataCSVlocation, fileFormat: ExcelFileFormat.XLSX, ListDelimiter: ",");
            WorkSheet sheet = workbook.DefaultWorkSheet;
            DataTable dt = sheet.ToDataTable(true);
            List<Movie> AllMovies = new List<Movie>();


            //create movies from datatable
            foreach (DataRow item in dt.Rows)
            {
                Movie movie = new Movie();
                try
                {
                    movie.Id = Convert.ToInt32(item["MovieId"]);
                    movie.movieId = Convert.ToInt32(item["MovieId"]);
                    movie.title = item["Title"].ToString();
                    movie.duration = item["Duration"].ToString();
                    movie.releaseYear = Convert.ToInt32(item["ReleaseYear"]);
                    movie.language = item["Language"].ToString();
                }
                catch (Exception)
                {
                    //skip adding movie metadata if any problems arise
                    continue;
                }

                AllMovies.Add(movie);

            }

            return AllMovies;
        }




        public IEnumerable<Stats> GetMovieStats()
        {
            WorkBook workbook = WorkBook.LoadCSV(statsCSVlocation, fileFormat: ExcelFileFormat.XLSX, ListDelimiter: ",");
            WorkSheet sheet = workbook.DefaultWorkSheet;
            DataTable dt = sheet.ToDataTable(true);
            List<MovieStat> AllMoviestats = new List<MovieStat>();


            foreach (DataRow item in dt.Rows)
            {
                MovieStat movie = new MovieStat();
                try
                {
                    movie.movieId = Convert.ToInt32(item["movieId"]);
                    movie.watchDurationMs = Convert.ToInt32(item["watchDurationMs"]);
                }
                catch (Exception)
                {
                    //skip adding movie metadata if any problems arise
                    continue;
                }
                AllMoviestats.Add(movie);
            }

            var grouped = 
            AllMoviestats
            .GroupBy(a => a.movieId).ToList();

            List<Stats> AllStats = new List<Stats>();
            foreach (IGrouping<int, MovieStat> item in grouped)
            {
                Stats stat = new Stats();
                stat.watches = item.Count();
                int sum = 0;
                foreach (MovieStat x in item)
                {
                    sum += x.watchDurationMs / 1000;
                    stat.movieId = x.movieId;
                }
                stat.averageWatchDurationS = (sum / stat.watches);
                AllStats.Add(stat);
            }
            
            List<Movie> AllMovies = GetAllMovies()
                .GroupBy(a => a.movieId)
                .Select(g => g.OrderByDescending(a => a.Id).First())
                .ToList();
                 
            
            List<Stats> ToDelete = new List<Stats>();

            foreach (Stats item in AllStats)
            {
                Movie _movie = AllMovies.Where(x => x.movieId == item.movieId).FirstOrDefault();
                if (_movie is null)
                {
                    ToDelete.Add(item);
                    continue;
                }
                item.title = _movie.title;
                item.averageWatchDurationS = item.averageWatchDurationS;
                item.releaseYear = _movie.releaseYear;
            }

            foreach (var item in ToDelete)
            {
                AllStats.Remove(item);
            }
            return AllStats.OrderByDescending(x => x.watches).ThenByDescending(x => x.releaseYear);
        }
    }
}
