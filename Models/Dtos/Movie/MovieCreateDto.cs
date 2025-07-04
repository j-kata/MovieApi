using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.Dtos.Movie
{
    public class MovieCreateDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        [Range(1800, 2100)]
        public int Year { get; set; }
        [Required]
        [Range(0, 24)]
        public int Hours { get; set; }
        [Required]
        [Range(0, 60)]
        public int Minutes { get; set; }
        [Required]
        public int GenreId { get; set; }
        [MaxLength(500)]
        public string? Synopsis { get; set; }
        [MaxLength(100)]
        public string? Language { get; set; }
        [Range(0, int.MaxValue)]
        public int? Budget { get; set; }
    }
}