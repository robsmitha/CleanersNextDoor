using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Customers;
using Application.Customers.Commands.CreateAddress;
using Application.Customers.Commands.CreateCartItem;
using Application.Customers.Commands.CreatePaymentMethod;
using Application.Customers.Commands.CreateServiceRequest;
using Application.Customers.Commands.DeleteAddress;
using Application.Customers.Commands.DeletePaymentMethod;
using Application.Customers.Commands.RemoveCartItem;
using Application.Customers.Commands.SetDefaultAddress;
using Application.Customers.Commands.SetDefaultPaymentMethod;
using Application.Customers.Queries.GetCustomer;
using Application.Customers.Queries.GetCustomerAddresses;
using Application.Customers.Queries.GetCustomerByEmail;
using Application.Customers.Queries.GetCustomerCart;
using Application.Customers.Queries.GetCustomerOrder;
using Application.Customers.Queries.GetOrderHistory;
using Application.Customers.Queries.GetPaymentMethods;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace CleanersNextDoor.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAppUserService _user;
        private readonly IIdentityService _identity;
        private readonly IStripeService _stripe;
        public CustomersController(IMediator mediator, IIdentityService identity, IAppUserService user, IStripeService stripe)
        {
            _mediator = mediator;
            _identity = identity;
            _user = user;
            _stripe = stripe;
        }

        #region AllowAnonymous Methods
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
        [HttpGet("account")]
        public async Task<CustomerModel> GetCustomer()
        {
            return await _mediator.Send(new GetCustomerQuery(_user.ClaimID)) ?? new CustomerModel();
        }
        #endregion

        #region Payment methods
        [HttpGet("GetPaymentMethods")]
        public async Task<List<PaymentMethodModel>> GetPaymentMethods()
        {
            return await _mediator.Send(new GetPaymentMethodsQuery(_user.ClaimID)) ?? new List<PaymentMethodModel>();
        }
        [HttpPost("AddPaymentMethod")]
        public async Task<ActionResult<CreatePaymentMethodModel>> AddPaymentMethod(CreatePaymentMethodModel data)
        {
            return Ok(await _mediator.Send(new CreatePaymentMethodCommand(_user.ClaimID, data)));
        }
        [HttpPost("RemovePaymentMethod")]
        public async Task<ActionResult<bool>> RemovePaymentMethod(DeletePaymentMethodModel data)
        {
            return Ok(await _mediator.Send(new DeletePaymentMethodCommand(data.ID, _user.ClaimID)));
        }
        [HttpPost("SetDefaultPaymentMethod")]
        public async Task<ActionResult<bool>> SetDefaultPaymentMethod(SetDefaultPaymentMethodModel data)
        {
            return Ok(await _mediator.Send(new SetDefaultPaymentMethodCommand(data.ID, _user.ClaimID)));
        }
        #endregion

        #region Addresses
        [HttpGet("GetAddresses")]
        public async Task<List<CustomerAddress>> GetAddresses()
        {
            return await _mediator.Send(new GetCustomerAddressesQuery(_user.ClaimID)) ?? new List<CustomerAddress>();
        }
        [HttpPost("AddAddress")]
        public async Task<ActionResult<bool>> AddAddress(CreateAddressModel data)
        {
            return Ok(await _mediator.Send(new CreateAddressCommand(_user.ClaimID, data)));
        }
        [HttpPost("RemoveAddress")]
        public async Task<ActionResult<bool>> RemoveAddress(DeleteAddressModel data)
        {
            return Ok(await _mediator.Send(new DeleteAddressCommand(data.ID, _user.ClaimID)));
        }
        [HttpPost("SetDefaultAddress")]
        public async Task<ActionResult<bool>> SetDefaultAddress(SetDefaultAddressModel data)
        {
            return Ok(await _mediator.Send(new SetDefaultAddressCommand(data.ID, _user.ClaimID)));
        }
        #endregion

        #region Cart
        [HttpGet("cart/{merchantId}/{allowCheckout}")]
        public async Task<ActionResult<CustomerCartModel>> GetCustomerCart(int merchantId, bool allowCheckout = false)
        {
            return await _mediator.Send(new GetCustomerCartQuery(_user.ClaimID, merchantId, allowCheckout));
        }
        [HttpPost("AddToCart")]
        public async Task<ActionResult<CreateCartItemModel>> AddToCart(CreateCartItemModel data)
        {
            return Ok(await _mediator.Send(new CreateCartItemCommand(data, _user.ClaimID)));
        }
        [HttpPost("RemoveCartItem")]
        public async Task<ActionResult<bool>> RemoveCartItem(RemoveCartItemModel data)
        {
            return await _mediator.Send(new RemoveCartItemCommand(data, _user.ClaimID));
        }
        [HttpPost("CreateServiceRequest")]
        public async Task<ActionResult<int>> CreateServiceRequest(CreateServiceRequestModel model)
        {
            return await _mediator.Send(new CreateServiceRequestCommand(model, _user.ClaimID));
        }
        #endregion

        [HttpPost("StripeClientSecret")]
        public IStripeClientSecret StripeClientSecret()
        {
            return _identity.GetStripeSecretKey(_user.ClaimID);
        }
        [HttpPost("StripePublicKey")]
        public IStripePublicKey StripePublicKey()
        {
            return _stripe.StripePublicKey();
        }

        [HttpPost("TryPayWithPaymentMethod")]
        public async Task<ActionResult<int>> TryPayWithPaymentMethod(CreateServiceRequestModel model)
        {
            PaymentIntent paymentIntent;
            try
            {
                //TODO: optimize/consolidate TryPayWithPaymentMethod
                var customer = await _mediator.Send(new GetCustomerQuery(_user.ClaimID));
                var cart = await _mediator.Send(new GetCustomerCartQuery(customer.ID, model.MerchantID));
                var amount = cart.Total;

                paymentIntent = _stripe.CreatePaymentIntent(new PaymentIntentCreateOptions
                {
                    Amount = Convert.ToInt64(amount) * 100,
                    Currency = "usd",
                    PaymentMethod = model.Payment.StripePaymentMethodID,
                    Customer = customer.StripeCustomerID,
                    // A PaymentIntent can be confirmed some time after creation,
                    // but here we want to confirm (collect payment) immediately.
                    Confirm = true,

                    // If the payment requires any follow-up actions from the
                    // customer, like two-factor authentication, Stripe will error
                    // and you will need to prompt them for a new payment method.
                    ErrorOnRequiresAction = true,
                });

            }
            catch (StripeException e)
            {
                return StatusCode(501, e.StripeError.Message);
            }

            if (paymentIntent?.Status == "succeeded")
            {
                // Handle post-payment fulfillment
                return await CreateServiceRequest(model);
            }
            else
            {
                // Any other status would be unexpected, so error
                return StatusCode(500, new { error = "Invalid PaymentIntent status" });
            }
        }

        [HttpGet("orderHistory")]
        public async Task<OrderHistoryModel> GetOrderHistory()
        {
            return await _mediator.Send(new GetOrderHistoryQuery(_user.ClaimID));
        }
        [HttpGet("getOrder/{id}")]
        public async Task<GetCustomerOrderModel> GetOrder(int id)
        {
            return await _mediator.Send(new GetCustomerOrderQuery(id, _user.ClaimID));
        }
    }
}