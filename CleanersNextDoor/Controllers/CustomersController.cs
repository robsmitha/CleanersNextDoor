using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Customers;
using Application.Customers.Commands.CreateAddress;
using Application.Customers.Commands.CreateCartItem;
using Application.Customers.Commands.CreateCustomer;
using Application.Customers.Commands.CreatePaymentMethod;
using Application.Customers.Commands.DeleteAddress;
using Application.Customers.Commands.DeletePaymentMethod;
using Application.Customers.Commands.RemoveCartItem;
using Application.Customers.Commands.SetDefaultAddress;
using Application.Customers.Commands.SetDefaultPaymentMethod;
using Application.Customers.Queries.CustomerSignIn;
using Application.Customers.Queries.GetCustomer;
using Application.Customers.Queries.GetCustomerAddresses;
using Application.Customers.Queries.GetCustomerByEmail;
using Application.Customers.Queries.GetCustomerCart;
using Application.Customers.Queries.GetPaymentMethods;
using Domain.Entities;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CleanersNextDoor.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthenticationService _auth;
        public CustomersController(IMediator mediator, IAuthenticationService auth)
        {
            _mediator = mediator;
            _auth = auth;
        }

        #region AllowAnonymous Methods
        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IApplicationUser> SignIn(CustomerModel data)
        {
            return await _mediator.Send(new CustomerSignInQuery(data.Email, data.Password)); ;
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IApplicationUser> SignUp(CustomerModel data)
        {
            return await _mediator.Send(new CreateCustomerCommand(data)); ;
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
        #endregion

        #region Customer
        [HttpGet("Profile")]
        public async Task<CustomerModel> GetCustomerProfile()
        {
            return await _mediator.Send(new GetCustomerQuery(_auth.ClaimID)) ?? new CustomerModel();
        }
        #endregion

        #region Payment methods
        [HttpGet("GetPaymentMethods")]
        public async Task<List<PaymentMethodModel>> GetPaymentMethods()
        {
            return await _mediator.Send(new GetPaymentMethodsQuery(_auth.ClaimID)) ?? new List<PaymentMethodModel>();
        }
        [HttpPost("AddPaymentMethod")]
        public async Task<ActionResult<CreatePaymentMethodModel>> AddPaymentMethod(CreatePaymentMethodModel data)
        {
            return Ok(await _mediator.Send(new CreatePaymentMethodCommand(_auth.ClaimID, data)));
        }
        [HttpPost("RemovePaymentMethod")]
        public async Task<ActionResult<bool>> RemovePaymentMethod(DeletePaymentMethodModel data)
        {
            return Ok(await _mediator.Send(new DeletePaymentMethodCommand(data.ID, _auth.ClaimID)));
        }
        [HttpPost("SetDefaultPaymentMethod")]
        public async Task<ActionResult<bool>> SetDefaultPaymentMethod(SetDefaultPaymentMethodModel data)
        {
            return Ok(await _mediator.Send(new SetDefaultPaymentMethodCommand(data.ID, _auth.ClaimID)));
        }
        #endregion

        #region Addresses
        [HttpGet("GetAddresses")]
        public async Task<List<CustomerAddress>> GetAddresses()
        {
            return await _mediator.Send(new GetCustomerAddressesQuery(_auth.ClaimID)) ?? new List<CustomerAddress>();
        }
        [HttpPost("AddAddress")]
        public async Task<ActionResult<CreateAddressModel>> AddAddress(CreateAddressModel data)
        {
            return Ok(await _mediator.Send(new CreateAddressCommand(_auth.ClaimID, data)));
        }
        [HttpPost("RemoveAddress")]
        public async Task<ActionResult<bool>> RemoveAddress(DeleteAddressModel data)
        {
            return Ok(await _mediator.Send(new DeleteAddressCommand(data.ID, _auth.ClaimID)));
        }
        [HttpPost("SetDefaultAddress")]
        public async Task<ActionResult<bool>> SetDefaultAddress(SetDefaultAddressModel data)
        {
            return Ok(await _mediator.Send(new SetDefaultAddressCommand(data.ID, _auth.ClaimID)));
        }
        #endregion

        #region Cart
        [HttpGet("cart/{merchantId}")]
        public async Task<ActionResult<CustomerCartModel>> GetCustomerCart(int merchantId)
        {
            return await _mediator.Send(new GetCustomerCartQuery(_auth.ClaimID, merchantId));
        }
        [HttpPost("AddToCart")]
        public async Task<ActionResult<CreateCartItemModel>> AddToCart(CreateCartItemModel data)
        {
            return Ok(await _mediator.Send(new CreateCartItemCommand(data, _auth.ClaimID)));
        }
        [HttpPost("RemoveCartItem")]
        public async Task<ActionResult<bool>> RemoveCartItem(RemoveCartItemModel data)
        {
            return await _mediator.Send(new RemoveCartItemCommand(data, _auth.ClaimID));
        }
        #endregion

    }
}