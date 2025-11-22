using CORE.APP.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class CountryResponse : Response
    {
        [DisplayName("Country Name")]
        [Display(Name = "Country")]
        public string CountryName { get; set; }

        public List<CityResponse> Cities { get; set; }
    }
}