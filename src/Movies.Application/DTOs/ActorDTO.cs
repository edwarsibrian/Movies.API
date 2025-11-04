using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.DTOs
{
    public class ActorDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public required string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? Picture { get; set; }
    }
}
