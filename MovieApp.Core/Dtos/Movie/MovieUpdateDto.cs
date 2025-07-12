using System.ComponentModel.DataAnnotations;

namespace MovieApp.Core.Dtos.Movie;

public class MovieUpdateDto : MovieCreateDto
{
    [Required]
    public int Id { get; set; }
}
