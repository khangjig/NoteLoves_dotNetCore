using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Models
{
    public class NoteImage
    {
        [Key]
        public int Id { get; set; }

        public byte[] Image { get; set; }

        [Timestamp]
        public DateTime CreatedAt { get; set; }
        
        public int NoteId { get; set; }

        public Note Notes { get; set; }
    }
}
