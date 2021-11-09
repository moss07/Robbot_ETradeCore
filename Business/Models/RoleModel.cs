using AppCore.Records;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.Models
{
    public class RoleModel :Record
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("Role")]
        public string Name { get; set; }
        public List<UserModel> Users { get; set; }
    }
}
