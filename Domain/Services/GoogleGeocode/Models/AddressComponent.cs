using System.Collections.Generic;

namespace Domain.Services.GoogleGeocode.Models
{
    public class AddressComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }
}
