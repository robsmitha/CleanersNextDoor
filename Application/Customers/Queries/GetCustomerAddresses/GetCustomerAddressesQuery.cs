using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Queries.GetCustomerAddresses
{

    public class GetCustomerAddressesQuery : IRequest<List<CustomerAddress>>
    {
        public int CustomerID { get; set; }
        public GetCustomerAddressesQuery(int customerId)
        {
            CustomerID = customerId;
        }
    }

    public class GetCustomerAddressesQueryHandler : IRequestHandler<GetCustomerAddressesQuery, List<CustomerAddress>>
    {
        private readonly IApplicationDbContext _context;
        public GetCustomerAddressesQueryHandler(IApplicationDbContext context) 
        {
            _context = context;
        }
        public async Task<List<CustomerAddress>> Handle(GetCustomerAddressesQuery request, CancellationToken cancellationToken)
        {
            var locations = await _context.CustomerAddresses
                .Where(p => p.CustomerID == request.CustomerID)
                .ToArrayAsync();
            if (locations == null || locations.Length == 0) return new List<CustomerAddress>();
            //TODO: revist grouping strategy to group by BaseAddress.Equals()
            var customerAddresses = new List<CustomerAddress>();
            customerAddresses.Add(locations[0]);
            for (int i = 1; i < locations.Length; i++)
                if (!locations[i].Equals(locations[i - 1]))
                    customerAddresses.Add(locations[i]);

            return customerAddresses;
        }
    }

}
