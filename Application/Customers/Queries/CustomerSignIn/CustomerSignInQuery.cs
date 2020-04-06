using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Queries.CustomerSignIn
{
    public class CustomerSignInQuery : IRequest<ApplicationUser>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public CustomerSignInQuery(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
    public class CustomerSignInQueryHandler : IRequestHandler<CustomerSignInQuery, ApplicationUser>
    {
        private readonly ICleanersNextDoorContext _context;
        private readonly IIdentityService _identity;
        private IMapper _mapper;

        public CustomerSignInQueryHandler(
            ICleanersNextDoorContext context,
            IIdentityService identity,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
            _identity = identity;
        }
        public async Task<ApplicationUser> Handle(CustomerSignInQuery request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers
                .SingleOrDefaultAsync(c => c.Email.ToLower() == request.Email.ToLower());
            return _identity.AuthenticateCustomer(customer, request.Password);
        }
    }
}
