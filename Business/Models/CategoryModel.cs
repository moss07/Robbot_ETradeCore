using AppCore.Records;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.Models
{
    public class CategoryModel : Record
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(400)]
        public string Description { get; set; }

        [DisplayName("Product Count")]
        public int ProductCount { get; set; }
    }
}
