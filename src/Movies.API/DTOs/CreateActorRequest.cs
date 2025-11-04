using System.ComponentModel.DataAnnotations;

namespace Movies.API.DTOs
{
    public class CreateActorRequest
    {
        [Required]
        [StringLength(150)]
        public required string ActorName { get; set; }

        public DateTime BirthDate { get; set; }

        public IFormFile? Picture { get; set; }
    }
}
