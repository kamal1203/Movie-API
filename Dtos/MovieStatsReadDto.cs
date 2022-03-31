namespace Movie_API.Dtos
{
    public class MovieStatsReadDto
    {
        public int movieId { get; set; }
        public string title { get; set; }
        public string averageWatchDurationS { get; set; }
        public int watches { get; set; }
        public string releaseYear { get; set; }
    }
}
