using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<CustomerModel>
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
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerModel>
    {
        private readonly ICleanersNextDoorContext _context;
        private IMapper _mapper;

        public CreateCustomerCommandHandler(
            ICleanersNextDoorContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CustomerModel> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = new Customer
            {
                Active = true,
                CreatedAt = DateTime.Now,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Email = request.Email,
                Password = request.Password,
                Phone = request.Phone
            };
            _context.Customers.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<CustomerModel>(entity);
        }
    }
}
