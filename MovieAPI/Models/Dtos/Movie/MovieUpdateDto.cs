using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models.Dtos.Movie
{
    public class MovieUpdateDto : MovieCreateDto
    {
        [Required]
        public int Id { get; set; }
    }
}