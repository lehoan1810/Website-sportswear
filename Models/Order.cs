using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace banhang.Models
{
    public class Order
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }
        public string Username { get; set; }
        public virtual User user { get; set; }
        public string DiaChi { get; set; }
        public string GhiChu { get; set; }
        public DateTime NgayDat { get; set; }
        public ICollection<Item> items { get; set; }
        public string TrangThai { get; set; }
        public double Tong { get; set; }
    }
}