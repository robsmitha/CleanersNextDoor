using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Queries.GetCustomerByEmail
{
    public class GetCustomerByEmailQuery : IRequest<CustomerModel>
    {
        public string Email { get; set; }
        public GetCustomerByEmailQuery(string email)
        {
            Email = email;
        }
    }
    public class GetCustomerByEmailQueryHandler : IRequestHandler<GetCustomerByEmailQuery, CustomerModel>
    {
        private readonly ICleanersNextDoorContext _context;
        private IMapper _mapper;

        public GetCustomerByEmailQueryHandler(
            ICleanersNextDoorContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerModel> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Customers.SingleOrDefaultAsync(c => c.Email.ToLower() == request.Email.ToLower());
            return entity != null
                ? _mapper.Map<CustomerModel>(entity)
                : new CustomerModel();
        }
    }
}
