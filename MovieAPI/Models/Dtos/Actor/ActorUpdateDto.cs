using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models.Dtos.Actor
{
    public class ActorUpdateDto : ActorCreateDto
    {
        [Required]
        public int Id { get; set; }
    }
}