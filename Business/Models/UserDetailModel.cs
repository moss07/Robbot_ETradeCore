using AppCore.Records;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class UserDetailModel : Record
    {
        [Required]
        [StringLength(200)]
        [EmailAddress]
        [DisplayName("E-Mail")]
        public string EMail { get; set; }
        [Required(ErrorMessage ="{0} is required!")]
        [DisplayName("Country")]
        public int CountryId { get; set; }
        public CountryModel Country { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("City")]
        public int CityId { get; set; }
        public CityModel City { get; set; }

        [Required]
        [StringLength(1000)]
        public string Address { get; set; }
    }
}
