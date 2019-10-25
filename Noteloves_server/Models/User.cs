using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Please enter your email")]
        [DataType(DataType.EmailAddress)]
        //[RegularExpression(@"a-z0-9", ErrorMessage ="Please enter correct email address")]
        [StringLength(50)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [StringLength(100)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter your name")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please choose your sex")]
        public Boolean Sex { get; set; }

        [Required(ErrorMessage = "Please choose your birthday")]
        public DateTime Birthday { get; set; }

        [StringLength(100)]
        public string RefreshToken { get; set; }

        [Timestamp]
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        
        public virtual Avatar Avatar{ get; set; }
    }
}
