using System.ComponentModel.DataAnnotations;
using MovieApi.Validations;

namespace MovieApi.Models.Dtos.Actor
{
    public class ActorCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MovieEra]
        public int BirthYear { get; set; }
    }
}