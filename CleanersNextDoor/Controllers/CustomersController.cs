using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Common.Utilities;
using Application.Customers;
using Application.Customers.Commands.CreateCartItem;
using Application.Customers.Commands.CreateCustomer;
using Application.Customers.Commands.RemoveCartItem;
using Application.Customers.Queries.GetCustomer;
using Application.Customers.Queries.GetCustomerByEmail;
using Application.Customers.Queries.GetCustomerCart;
using Application.LineItems;
using FluentValidation;
using MediatR;
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
        public async Task<CustomerModel> GetCustomer(int id)
        {
            var customer = await _mediator.Send(new GetCustomerQuery(id));
            if (customer != null)
            {
                return customer;
            }
            return new CustomerModel();
        }
        [HttpGet("CheckEmailAvailability/{email}")]
        public async Task<dynamic> CheckEmailAvailability(string email)
        {
            var customer = await _mediator.Send(new GetCustomerByEmailQuery(email));
            return new
            {
                isAvailable = customer == null || customer?.ID == 0
            };
        }
        [HttpGet("{customerId}/cart/{merchantId}")]
        public async Task<ActionResult<CustomerCartModel>> GetCustomerCart(int customerId, int merchantId)
        {
            return await _mediator.Send(new GetCustomerCartQuery(customerId, merchantId));
        }
        [HttpPost("{id}/AddToCart")]
        public async Task<ActionResult<CreateCartItemModel>> AddToCart(CreateCartItemModel data)
        {
            try
            {
                return Ok(await _mediator.Send(new CreateCartItemCommand(data)));
            }
            catch (ValidationException e)
            {
                var errors = JsonSerializer.Serialize(e.Errors.Select(x => x.ErrorMessage));
                return StatusCode(500, errors);
            }
        }
        [HttpPost("{id}/RemoveCartItem")]
        public async Task<bool> RemoveCartItem(RemoveCartItemModel data)
        {
            return await _mediator.Send(new RemoveCartItemCommand(data));
        }
    }
}