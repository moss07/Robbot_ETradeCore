using AppCore.Records;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Entities
{
    public class Role : Record
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public List<User> Users { get; set; }
    }
}
