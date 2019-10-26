using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title not null")]
        [StringLength(50)]
        public string Title { get; set; }
        
        public string Content { get; set; }

        [Required]
        public bool Status { get; set; }

        [Timestamp]
        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }

        public User Users { get; set; }
    }
}
