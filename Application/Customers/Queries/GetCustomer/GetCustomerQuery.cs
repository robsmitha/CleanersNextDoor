using Application.Customers;
using AutoMapper;
using Domain.Models;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetUser
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
            return entity != null
                ? _mapper.Map<CustomerModel>(entity)
                : new CustomerModel();
        }
    }
}
