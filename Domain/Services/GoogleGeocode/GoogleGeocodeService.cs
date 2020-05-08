using Domain.Services.Configuration.Interfaces;
using Domain.Services.Configuration.Models;
using Domain.Services.GoogleGeocode.Interfaces;
using Domain.Services.GoogleGeocode.Models;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Domain.Services.GoogleGeocode
{
    public class GoogleGeocodeService : IGoogleGeocodeService
    {
        public const string ApiBaseUrl = "https://maps.googleapis.com/maps/api/geocode/json?";
        private readonly IAppSettings _appSettings;
        public GoogleGeocodeService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public async Task<Result> Geocode(string address)
        {
            var response = await SendAsync<Response>($"{ApiBaseUrl}address={address}");
            var result = response.results.FirstOrDefault();
            return result;
        }

        public async Task<Result> ReverseGeocode(double lat, double lng)
        {
            var response = await SendAsync<Response>($"{ApiBaseUrl}latlng={lat},{lng}");
            var result = response.results.FirstOrDefault();
            return result;
        }
        public async Task<T> SendAsync<T>(string requestUri)
        {
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await client.GetAsync($"{requestUri}&key={_appSettings.GoogleApiKey}");
            var result = response.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<T>(result);
        }
    }
}
