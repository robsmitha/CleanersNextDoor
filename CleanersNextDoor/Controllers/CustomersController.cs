using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Utilities;
using Application.Customers;
using Application.Customers.Commands.CreateCustomerCommand;
using Application.Customers.Queries.GetCustomerByEmail;
using Application.Users.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanersNextDoor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetByEmail/{email}")]
        public async Task<ActionResult<CustomerModel>> GetCustomerByEmail(string email)
        {
            return await _mediator.Send(new GetCustomerByEmailQuery(email));
        }
        [HttpPost("SignIn")]
        public async Task<CustomerModel> SignIn(CustomerModel data)
        {
            var customer = await _mediator.Send(new GetCustomerByEmailQuery(data.Email));
            if (!string.IsNullOrEmpty(customer?.Password) && SecurePasswordHasher.Verify(data.Password, customer.Password))
            {
                return customer;
            }
            return data;
        }
        [HttpPost("SignUp")]
        public async Task<CustomerModel> SignUp(CustomerModel data)
        {
            data.Password = SecurePasswordHasher.Hash(data.Password);
            return await _mediator.Send(new CreateCustomerCommand(data));
        }
        [HttpGet("{id}")]
        public async Task<CustomerModel> GetUser(int id)
        {
            var customer = await _mediator.Send(new GetCustomerQuery(id));
            if (customer != null)
            {
                return customer;
            }
            return new CustomerModel();
        }
        [HttpGet("CheckEmailAvailability/{email}")]
        public async Task<dynamic> CheckUsernameAvailability(string email)
        {
            var customer = await _mediator.Send(new GetCustomerByEmailQuery(email));
            return new
            {
                isAvailable = customer == null || customer?.ID == 0
            };
        }
    }
}