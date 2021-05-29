using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace banhang.Models
{
    public class Item
    {
        [Key,Column(Order = 1)]
        public int ItemId { get; set; }
        [Key, Column(Order = 2)]
        public int OrderId { get; set; }

        public int Quantity { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}