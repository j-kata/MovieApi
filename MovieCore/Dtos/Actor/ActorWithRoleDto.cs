using System.ComponentModel.DataAnnotations;

namespace MovieCore.Dtos.Actor
{
    public class ActorWithRoleDto : ActorDto
    {
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}