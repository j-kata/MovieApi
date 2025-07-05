using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.Dtos.Actor
{
    public class ActorCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int BirthYear { get; set; }
    }
}