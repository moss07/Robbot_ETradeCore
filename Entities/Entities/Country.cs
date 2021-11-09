using AppCore.Records;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Entities
{
    public class Country : Record
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public List<City> Cities { get; set; }
        public List<UserDetail> UserDetails { get; set; }
    }
}
