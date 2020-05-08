using System.Collections.Generic;

namespace Application.Common.Models
{
    public class Response
    {
        public List<Result> results { get; set; }
        public string status { get; set; }
    }
}
