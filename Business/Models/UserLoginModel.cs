using AppCore.Records;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.Models
{
    public class UserLoginModel : Record
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(10)]
        public string Password { get; set; }

        public string RoleName { get; set; }
    }
}
