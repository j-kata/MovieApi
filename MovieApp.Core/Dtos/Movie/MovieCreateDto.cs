using System.ComponentModel.DataAnnotations;
using MovieApp.Core.Validations;

namespace MovieApp.Core.Dtos.Movie;

public class MovieCreateDto
{
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MovieEra(1888, 20)]
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
