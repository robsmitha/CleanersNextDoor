using FluentValidation;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandValidator
        : AbstractValidator<CreateCustomerCommand>
    {
        private readonly ICleanersNextDoorContext _context;
        public CreateCustomerCommandValidator(ICleanersNextDoorContext context)
        {
            _context = context;
            RuleFor(v => v.Email)
                .MaximumLength(50)
                .NotEmpty()
                .MustAsync(BeUniqueEmail)
                    .WithMessage("The specified username already exists.");
        }
        public async Task<bool> BeUniqueEmail(string email,
            CancellationToken cancellationToken)
        {
            return await _context.Customers
                .AllAsync(l => l.Email.ToLower() != email.ToLower());
        }
    }
}
