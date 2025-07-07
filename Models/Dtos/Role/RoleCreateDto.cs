using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.Dtos.Movie
{
    public class RoleCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
    }
}