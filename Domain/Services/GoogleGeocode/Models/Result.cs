using System.Collections.Generic;

namespace Domain.Services.GoogleGeocode.Models
{
    public class Result
    {
        public List<AddressComponent> address_components { get; set; }
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public string place_id { get; set; }
        public PlusCode plus_code { get; set; }
        public List<string> types { get; set; }
    }
}
