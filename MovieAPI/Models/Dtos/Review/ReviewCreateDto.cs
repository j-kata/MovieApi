using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models.Dtos.Review
{
    public class ReviewCreateDto
    {
        [Required]
        public int MovieId { get; set; }
        [Required]
        [MaxLength(100)]
        public string ReviewerName { get; set; } = string.Empty;
        [Required]
        [MaxLength(1500)]
        public string Comment { get; set; } = string.Empty;
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
    }
}