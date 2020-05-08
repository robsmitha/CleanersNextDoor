using Domain.Services.GoogleGeocode.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.GoogleGeocode.Interfaces
{
    public interface IGoogleGeocodeService
    {
        Task<Result> Geocode(string address);
        Task<Result> ReverseGeocode(double lat, double lng);
    }
}
