using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.Dtos.Movie
{
    public class MovieUpdateDto : MovieCreateDto
    {
        [Required]
        public int Id { get; set; }
    }
}