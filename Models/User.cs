using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace banhang.Models
{
    public class User
    {
        [Key, Column(Order = 1)]
        [Required(ErrorMessage ="Không được để trống")]
        public string Username { get; set; }
        [Required(ErrorMessage ="Không được để trống")]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }

        public int Lever { get; set; }
    }

}