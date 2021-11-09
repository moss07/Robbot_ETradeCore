using AppCore.Records;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.Models
{
    public class CountryModel : Record
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(200, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("Country Name")]
        public string Name { get; set; }
    }
}
