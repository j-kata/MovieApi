using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.Dtos.Actor
{
    public class ActorUpdateDto : ActorCreateDto
    {
        [Required]
        public int Id { get; set; }
    }
}