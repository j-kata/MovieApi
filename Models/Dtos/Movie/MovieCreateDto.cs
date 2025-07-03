using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.Dtos.Movie
{
    public class MovieCreateDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Range(1800, 2100)]
        public int Year { get; set; }
        [Range(0, 24)]
        public int Hours { get; set; }
        [Range(0, 60)]
        public int Minutes { get; set; }
        [Required]
        public int GenreId { get; set; }
        public string Synopsis { get; set; } = string.Empty; // TODO: make nullable
        public string Language { get; set; } = string.Empty; // TODO: make nullable
        public int Budget { get; set; }
    }
}