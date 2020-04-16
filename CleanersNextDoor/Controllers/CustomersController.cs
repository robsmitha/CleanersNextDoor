using System.Threading.Tasks;
using Application.Customers;
using Application.Customers.Commands.CreateAddress;
using Application.Customers.Commands.CreateCartItem;
using Application.Customers.Commands.CreateCustomer;
using Application.Customers.Commands.CreatePaymentMethod;
using Application.Customers.Commands.DeleteAddress;
using Application.Customers.Commands.RemoveCartItem;
using Application.Customers.Queries.CustomerSignIn;
using Application.Customers.Queries.GetCustomer;
using Application.Customers.Queries.GetCustomerByEmail;
using Application.Customers.Queries.GetCustomerCart;
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

        #region Account Methods
        [HttpGet("Profile")]
        public async Task<CustomerModel> GetCustomerProfile()
        {
            return await _mediator.Send(new GetCustomerQuery(_auth.ClaimID)) ?? new CustomerModel();
        }
        [HttpPost("SignOut")]
        public ActionResult<bool> SignOut()
        {
            _auth.SetAuthentication(null);
            return true;
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
        [HttpPost("AddPaymentMethod")]
        public async Task<ActionResult<CreatePaymentMethodModel>> AddPaymentMethod(CreatePaymentMethodModel data)
        {
            return Ok(await _mediator.Send(new CreatePaymentMethodCommand(_auth.ClaimID, data)));
        }
        #endregion

        #region Cart Methods
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