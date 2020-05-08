using System.Collections.Generic;

namespace Domain.Services.GoogleGeocode.Models
{
    public class Response
    {
        public List<Result> results { get; set; }
        public string status { get; set; }
    }
}
