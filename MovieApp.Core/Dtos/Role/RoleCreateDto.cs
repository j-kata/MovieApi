using System.ComponentModel.DataAnnotations;

namespace MovieApp.Core.Dtos.Role;

public class RoleCreateDto
{
    [Required]
    public int ActorId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Role { get; set; } = string.Empty;
}
