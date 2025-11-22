using CORE.APP.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class CityResponse : Response
    {
        [DisplayName("City Name")]
        [Display(Name = "City")]
        public string CityName { get; set; }

        public CountryResponse Country { get; set; }
    }
}