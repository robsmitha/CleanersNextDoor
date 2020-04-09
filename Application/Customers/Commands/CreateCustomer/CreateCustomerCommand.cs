using AutoMapper;
using Domain.Entities;
using Domain.Utilities;
using Infrastructure.Data;
using Infrastructure.Identity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<IApplicationUser>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public CreateCustomerCommand(CustomerModel model)
        {
            FirstName = model.FirstName;
            MiddleName = model.MiddleName;
            LastName = model.LastName;
            Email = model.Email;
            Password = model.Password;
            Phone = model.Phone;
        }
    }
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, IApplicationUser>
    {
        private readonly ICleanersNextDoorContext _context;
        private IIdentityService _identity;

        public CreateCustomerCommandHandler(
            ICleanersNextDoorContext context,
            IIdentityService identity
            )
        {
            _context = context;
            _identity = identity;
        }

        public async Task<IApplicationUser> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Email = request.Email,
                //todo: hash w customer.secret
                Password = SecurePasswordHasher.Hash(request.Password), 
                Phone = request.Phone
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync(cancellationToken);
            return _identity.AuthenticateCustomer(customer, request.Password);
        }
    }
}
