using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.Dtos.Movie
{
    public class RoleCreateDto
    {
        [Required]
        public int ActorId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Role { get; set; } = string.Empty;
    }
}