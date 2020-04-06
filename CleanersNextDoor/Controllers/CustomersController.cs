using System;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Customers;
using Application.Customers.Commands.CreateCartItem;
using Application.Customers.Commands.CreateCustomer;
using Application.Customers.Commands.RemoveCartItem;
using Application.Customers.Queries.CustomerSignIn;
using Application.Customers.Queries.GetCustomer;
using Application.Customers.Queries.GetCustomerByEmail;
using Application.Customers.Queries.GetCustomerCart;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanersNextDoor.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthService _auth;
        public CustomersController(IMediator mediator, IAuthService auth)
        {
            _mediator = mediator;
            _auth = auth;
        }

        private int CustomerID => int.TryParse(_auth.ClaimID, out var id) ? id : 0;

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<ApplicationUser> SignIn(CustomerModel data)
        {
            var appUser = await _mediator.Send(new CustomerSignInQuery(data.Email, data.Password));
            return appUser;
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<CustomerModel> SignUp(CustomerModel data)
        {
            data.Password = SecurePasswordHasher.Hash(data.Password);
            var newCustomer = await _mediator.Send(new CreateCustomerCommand(data));
            return newCustomer;
        }

        [AllowAnonymous]
        [HttpGet("CheckEmailAvailability/{email}")]
        public async Task<dynamic> CheckEmailAvailability(string email)
        {
            var customer = await _mediator.Send(new GetCustomerByEmailQuery(email));
            return new
            {
                isAvailable = customer == null || customer?.ID == 0
            };
        }

        [HttpGet("profile")]
        public async Task<CustomerModel> GetCustomerProfile()
        {
            return await _mediator.Send(new GetCustomerQuery(CustomerID)) ?? new CustomerModel();
        }

        [HttpGet("cart/{merchantId}")]
        public async Task<ActionResult<CustomerCartModel>> GetCustomerCart(int merchantId)
        {
            return await _mediator.Send(new GetCustomerCartQuery(CustomerID, merchantId));
        }

        [HttpPost("AddToCart")]
        public async Task<ActionResult<CreateCartItemModel>> AddToCart(CreateCartItemModel data)
        {
            try
            {
                return Ok(await _mediator.Send(new CreateCartItemCommand(data, CustomerID)));
            }
            catch (ValidationException e)
            {
                var errors = JsonSerializer.Serialize(e.Errors);
                return StatusCode(500, errors);
            }
        }
        [HttpPost("RemoveCartItem")]
        public async Task<ActionResult<bool>> RemoveCartItem(RemoveCartItemModel data)
        {
            try
            {
                return await _mediator.Send(new RemoveCartItemCommand(data, CustomerID));
            }
            catch (ValidationException e)
            {
                var errors = JsonSerializer.Serialize(e.Errors);
                return StatusCode(500, errors);
            }
        }
    }
}