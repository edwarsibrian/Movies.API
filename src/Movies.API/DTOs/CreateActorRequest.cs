using System.ComponentModel.DataAnnotations;

namespace Movies.API.DTOs
{
    public class CreateActorRequest
    {
        [Required]
        [StringLength(150)]
        public required string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public IFormFile? Picture { get; set; }
    }
}
