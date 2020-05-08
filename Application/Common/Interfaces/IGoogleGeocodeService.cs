using Application.Common.Models;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IGoogleGeocodeService
    {
        Task<Result> Geocode(string address);
        Task<Result> ReverseGeocode(double lat, double lng);
    }
}
