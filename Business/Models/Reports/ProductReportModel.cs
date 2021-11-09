using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Business.Models.Reports
{
    public class ProductReportModel
    {
        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [DisplayName("Product Description")]
        public string ProductDescription { get; set; }

        [DisplayName("Unit Price")]
        public string UnitPriceText { get; set; }

        [DisplayName("Stock Amount")]
        public int StockAmount { get; set; }

        [DisplayName("Expiration Date")]
        public string ExpirationDateText { get; set; }

        [DisplayName("Category")]
        public string CategoryName { get; set; }

        [DisplayName("Category Description")]
        public string CategoryDescription { get; set; }
        public int CategoryId { get; set; }

        public double UnitPrice { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
