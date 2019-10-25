using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Models
{
    public class Avatar
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Student")]
        public int UserId { get; set; }

        public byte[] Image { get; set; }

        [Timestamp]
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdateAt { get; set; }
        
        public virtual User User { get; set; }
    }
}
