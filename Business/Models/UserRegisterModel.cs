using AppCore.Records;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class UserRegisterModel : Record
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
        [Required]
        [MinLength(3)]
        [MaxLength(10)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public UserDetailModel UserDetail { get; set; }

        public bool Active { get; set; }
    }
}
