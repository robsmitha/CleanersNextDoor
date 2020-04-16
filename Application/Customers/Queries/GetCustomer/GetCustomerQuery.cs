using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Identity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Queries.GetCustomer
{
    public class GetCustomerQuery : IRequest<CustomerModel>
    {
        public int CustomerID { get; set; }
        public GetCustomerQuery(int customerId)
        {
            CustomerID = customerId;
        }
    }

    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerModel>
    {
        private readonly ICleanersNextDoorContext _context;
        private IMapper _mapper;

        public GetCustomerQueryHandler(
            ICleanersNextDoorContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerModel> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Customers.FindAsync(request.CustomerID);
            if (entity == null) return new CustomerModel();

            var customer = _mapper.Map<CustomerModel>(entity);
            customer.Addresses = _context.CustomerAddresses
                .Where(a => a.CustomerID == request.CustomerID)
                .ToList();
            customer.PaymentMethods = _context.PaymentMethods
                .Where(p => p.CustomerID == request.CustomerID)
                .ToList();
            return customer;
        }
    }
}
