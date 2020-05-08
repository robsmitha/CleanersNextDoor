namespace Domain.Services.GoogleGeocode.Models
{
    public class Geometry
    {
        public Location location { get; set; }
        public string location_type { get; set; }
        public Viewport viewport { get; set; }
    }
}
