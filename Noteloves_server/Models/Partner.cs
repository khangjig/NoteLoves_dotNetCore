using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Models
{
    public class Partner
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter his/her name")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please choose his/her birthday")]
        public DateTime Birthday { get; set; }

        public byte[] Avatar { get; set; }

        public virtual User User { get; set; }
    }
}
