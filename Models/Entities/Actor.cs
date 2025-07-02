using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public int BirthYear { get; set; }
        public ICollection<Movie> Movies { get; set; } = [];
    }
}
