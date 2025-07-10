using System.ComponentModel.DataAnnotations;

namespace MovieApp.Core.Dtos.Actor
{
    public class ActorWithRoleDto : ActorDto
    {
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}