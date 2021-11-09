using System;
using System.ComponentModel.DataAnnotations;
using AppCore.Records;

namespace Entities.Entities
{
    public class Product : Record
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public double UnitPrice { get; set; }

        public int StockAmount { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [StringLength(255)]
        public string ImagePath { get; set; }
    }
}
