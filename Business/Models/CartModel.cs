using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Business.Models
{
    public class CartModel
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }

        [DisplayName("Product")]
        public string ProductName { get; set; } 

        [DisplayName("Unit Price")]
        public double UnitPrice { get; set; }
    }
}
