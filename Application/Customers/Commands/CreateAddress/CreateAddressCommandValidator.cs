using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Commands.CreateAddress
{
    public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
    {
        //Inject SS address service
        private readonly ICleanersNextDoorContext _context;
        private readonly IIdentityService _identity;
        private readonly List<CustomerAddress> customerAddresses;
        public CreateAddressCommandValidator(ICleanersNextDoorContext context,
            IIdentityService identity)
        {
            _context = context;
            _identity = identity;

            customerAddresses = _context.CustomerAddresses
                   .Where(a => a.CustomerID == _identity.ClaimID)
                   .ToList();


            #region Standard validations
            RuleFor(l => l.Street1).MaximumLength(50)
                .WithMessage("Maximum street 1 length excceeded.");
            RuleFor(l => l.City).MaximumLength(50)
                .WithMessage("Maximum city length excceeded.");
            RuleFor(l => l.StateAbbreviation).MaximumLength(2)
                .WithMessage("Maximum state abbreviation length excceeded.");
            RuleFor(l => l.Zip).MaximumLength(10)
                .WithMessage("Maximum zip code length excceeded.");
            #endregion

            #region Custom validations
            
            RuleFor(l => l.Name).MustAsync(HasUniqueName)
                    .WithMessage("An address with this name already exists on your account. Please use a different address name.");
            
            RuleFor(l => l.Street1).MustAsync(HasUniqueLocation)
                    .WithMessage("An address with this location information already exists on your account.");
            
            RuleFor(l => l.Street1).MustAsync(BeDeliverableAddress)
                    .WithMessage("The address could not be determined to be a deliverable address. Please review your entries and try again.");
            #endregion
        }

        /// <summary>
        /// Checks address location information is unique
        /// </summary>
        /// <param name="args"></param>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> HasUniqueLocation(CreateAddressCommand args, string name, CancellationToken cancellationToken)
        {
            await Task.FromResult(0);
            var location = new CustomerAddress
            {
                Street1 = args.Street1,
                Street2 = args.Street2,
                City = args.City,
                StateAbbreviation = args.StateAbbreviation,
                Zip = args.Zip,
            };

            return !customerAddresses.Any(a => a.Equals(location));
        }

        /// <summary>
        /// Checks address has a unique name within CustomerAddresses
        /// </summary>
        /// <param name="args"></param>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> HasUniqueName(CreateAddressCommand args, string name, CancellationToken cancellationToken)
        {
            await Task.FromResult(0);
            return string.IsNullOrEmpty(args.Name) 
                || !customerAddresses.Any(a => a.Name.ToLower() == args.Name.ToLower());
        }

        public async Task<bool> BeDeliverableAddress(CreateAddressCommand args, string street1,
            CancellationToken cancellationToken)
        {
            await Task.FromResult(0);
            //TODO: check delivery address

            return true;
        }
    }
}
