using AppCore.Records;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class CityModel : Record
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(200, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("City")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Country")]
        public int CountryId { get; set; }

        public CountryModel Country { get; set; }
    }
}
