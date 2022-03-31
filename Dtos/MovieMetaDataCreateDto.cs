using System.ComponentModel.DataAnnotations;

namespace Movie_API.Dtos
{
    public class MovieMetaDataCreateDto
    {
        [Required]
        public int movieId { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string language { get; set; }

        [Required]
        public string duration { get; set; }

        [Required]
        public int releaseYear { get; set; }

    }
}
