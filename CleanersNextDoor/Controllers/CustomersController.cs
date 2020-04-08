using System.Threading.Tasks;
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

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<ApplicationUser> SignIn(CustomerModel data)
        {
            return await _mediator.Send(new CustomerSignInQuery(data.Email, data.Password)); ;
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<ApplicationUser> SignUp(CustomerModel data)
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

        [HttpGet("profile")]
        public async Task<CustomerModel> GetCustomerProfile()
        {
            return await _mediator.Send(new GetCustomerQuery(_auth.ClaimID)) ?? new CustomerModel();
        }

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
        [HttpPost("SignOut")]
        public ActionResult<bool> SignOut()
        {
            _auth.SetAuthentication(null);
            return true;
        }
    }
}