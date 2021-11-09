using AppCore.Records;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Entities
{
    public class User : Record
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string UserName { get; set; }
        [Required]
        [StringLength(10)]
        public string Password { get; set; }
        public bool Active { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public int UserDetailId { get; set; }
        public UserDetail UserDetail { get; set; }
    }
}
