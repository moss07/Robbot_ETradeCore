using AppCore.Records;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Entities
{
    public class City : Record
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public List<UserDetail> UserDetails { get; set; }
    }
}
