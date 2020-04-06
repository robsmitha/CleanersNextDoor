using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Identity;
using MediatR;
using System;
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
        private readonly IAuthService _auth;
        private IMapper _mapper;

        public GetCustomerQueryHandler(
            ICleanersNextDoorContext context,
            IAuthService auth,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
            _auth = auth;
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
