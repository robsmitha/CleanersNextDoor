using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Merchants.Queries.GetMerchant
{
    public class MerchantLocationModel : IMapFrom<MerchantLocation>
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string StateAbbreviation { get; set; }
        public string Zip { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Phone { get; set; }
        public string OperatingHours { get; set; }
        public string ContactEmail { get; set; }
        public string CorrespondenceTypeName { get; set; }
        public string CorrespondenceTypeDescription { get; set; }
        public bool IsDefault { get; set; }
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
