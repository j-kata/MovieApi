using System.ComponentModel.DataAnnotations;

namespace MovieCore.Dtos.Actor
{
    public class ActorUpdateDto : ActorCreateDto
    {
        [Required]
        public int Id { get; set; }
    }
}