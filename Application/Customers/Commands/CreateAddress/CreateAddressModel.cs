using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Customers.Commands.CreateAddress
{
    public class CreateAddressModel : IMapFrom<CustomerAddress>
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
        public string Note { get; set; }
        public bool IsDefault { get; set; }
    }
}
