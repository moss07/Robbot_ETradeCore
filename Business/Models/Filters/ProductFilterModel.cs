using AppCore.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.Models.Filters
{
    public class ProductFilterModel
    {
        [DisplayName("Category")]
        public int? CategoryId { get; set; }

        [DisplayName("Product Name")]
        public string ProductName { get; set; }


        [DisplayName("Unit Price")]
        [StringDecimal("Unit price start value must be a decimal number!")]
        public string UnitPriceStartText { get; set; }

        [StringDecimal("Unit price end value must be a decimal number!")]
        public string UnitPriceEndText { get; set; }

        [DisplayName("Stock Amount")]
        public int? StockAmountStart { get; set; }
        public int? StockAmountEnd { get; set; }

        [DisplayName("Expiration Date")]
        public string ExpirationDateStartText { get; set; }
        public string ExpirationDateEndText { get; set; }
    }
}
