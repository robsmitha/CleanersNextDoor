using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        private readonly IApplicationDbContext _context;
        private IMapper _mapper;

        public GetCustomerByEmailQueryHandler(
            IApplicationDbContext context,
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
