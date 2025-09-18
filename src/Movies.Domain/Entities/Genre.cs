using System.ComponentModel.DataAnnotations;

namespace Movies.Domain.Entities
{
    public class Genre
    {
        public Genre()
        {
        }

        public int Id { get; set; }

        [StringLength(50)]
        public required string Name { get; set; }
    }
}
