using AppCore.Records;
using System.ComponentModel.DataAnnotations;

namespace Entities.Entities
{
    public class UserDetail : Record
    {
        [Required]
        [StringLength(200)]
        public string EMail { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }

        [Required]
        [StringLength(1000)]
        public string Address { get; set; }
        public User User { get; set; }
    }
}
