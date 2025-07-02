using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        public ICollection<Movie> Movies { get; set; } = [];
    }
}
