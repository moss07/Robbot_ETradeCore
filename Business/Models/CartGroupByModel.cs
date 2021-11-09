using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Business.Models
{
    public class CartGroupByModel
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }

        [DisplayName("Product")]
        public string ProductName { get; set; }

        [DisplayName("Total Unit Price")]
        public string TotalUnitPriceText { get; set; }

        [DisplayName("Total Product Count")]
        public int TotalProductCount { get; set; }
    }
}
