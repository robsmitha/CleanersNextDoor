using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Merchants.Queries.SearchMerchants
{
    public class SearchMerchantLocationModel : IMapFrom<MerchantLocation>
    {
        public int ID { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string StateAbbreviation { get; set; }
        public string Zip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? MilesAway { get; set; }
        public string DistanceAway
        {
            get
            {
                if (MilesAway == null) return string.Empty;

                //todo: globalization
                return $"{MilesAway:0.0} Miles away";
            }
        }
        public string Location
        {
            get
            {
                var fields = new[] { Street1, Street2, City, StateAbbreviation, Zip };
                var sb = new StringBuilder();
                for (var i = 0; i < fields.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(fields[i]))
                        continue;

                    sb.Append($"{fields[i]}, ");
                }
                var location = sb.ToString().Trim();
                return location.EndsWith(",")
                    ? location[0..^1]
                    : location;
            }
        }

    }
}
