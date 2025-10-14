using System.ComponentModel.DataAnnotations;

namespace Movies.Domain.Entities
{
    public class Actor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public required string Name { get; set; }

        public DateTime DateOfBirth { get; set; }
                
        public string? Picture { get; set; }
    }
}
