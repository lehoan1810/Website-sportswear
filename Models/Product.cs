using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace banhang.Models
{
    public class Product
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Không được để trống")]
        public int idProduct { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public int Price { get; set; }
        public string Category { get; set; }

        public string SubCategory { get; set; }

        public string Detail { get; set; }

        public DateTime NgayThem { get; set; }
        public string Image { get; set; }
    }
}