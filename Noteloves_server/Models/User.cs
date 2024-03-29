﻿using System;
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

        [Required(ErrorMessage = "Please enter your email")]
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

        [Required(ErrorMessage = "Please choose your birthday")]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        public DateTime LoveDate { get; set; }

        public int PartnerId { get; set; }

        [Required]
        [StringLength(100)]
        public string SyncCode { get; set; }

        [StringLength(100)]
        public string RefreshToken { get; set; }

        [Timestamp]
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
        
        public virtual Avatar Avatar{ get; set; }

        public List<Note> Notes { get; set; }

        public List<Notification> Notifications { get; set; }

        public virtual Partner Partner { get; set; }
    }
}
