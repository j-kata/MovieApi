using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models.Dtos.Actor
{
    public class ActorWithRoleDto : ActorDto
    {
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}