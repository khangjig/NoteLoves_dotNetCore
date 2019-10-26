using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Title not null")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content not null")]
        public string Content { get; set;}

        [Required(ErrorMessage = "Anniversary not null")]
        public DateTime Anniversary { get; set; }

        [Required]
        public bool Hidden { get; set; }

        [Timestamp]
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int UserId { get; set; }

        public User Users { get; set; }

        public List<NoteImage> NoteImages { get; set; }
    }
}
