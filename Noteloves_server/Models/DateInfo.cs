using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Models
{
    public class DateInfo
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        
        public int PartnerId { get; set; }

        public string Status { get; set; }

        [Timestamp]
        public DateTime CreatedAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public virtual User User { get; set; }
    }
}
