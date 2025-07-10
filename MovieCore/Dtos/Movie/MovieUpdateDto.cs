using System.ComponentModel.DataAnnotations;

namespace MovieCore.Dtos.Movie
{
    public class MovieUpdateDto : MovieCreateDto
    {
        [Required]
        public int Id { get; set; }
    }
}