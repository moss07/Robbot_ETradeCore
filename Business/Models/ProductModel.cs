using AppCore.Records;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.Models
{
    public class ProductModel : Record
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [DisplayName("Unit Price")]
        public double UnitPrice { get; set; }
        [DisplayName("Unit Price")]
        [Required]
        public string UnitPriceText { get; set; }

        [DisplayName("Stock Amount")]
        [Required]
        [Range(0, 9999, ErrorMessage = "{0} must be between {1} and {2}!")]
        public int StockAmount { get; set; }

        [DisplayName("Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

        [DisplayName("Expiration Date")]
        public string ExpirationDateText { get; set; }

        [DisplayName("Category")]
        [Required]
        public int CategoryId { get; set; }
        public CategoryModel Category { get; set; }

        [DisplayName("Image")]
        public string ImagePath { get; set; }
    }
}
