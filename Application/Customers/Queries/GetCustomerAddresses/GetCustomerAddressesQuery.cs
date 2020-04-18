using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly ICleanersNextDoorContext _context;
        public GetCustomerAddressesQueryHandler(ICleanersNextDoorContext context) 
        {
            _context = context;
        }
        public async Task<List<CustomerAddress>> Handle(GetCustomerAddressesQuery request, CancellationToken cancellationToken)
        {
            var customerAddresses = await _context.CustomerAddresses
                .Where(p => p.CustomerID == request.CustomerID)
                .ToListAsync();

            return customerAddresses;
        }
    }

}
