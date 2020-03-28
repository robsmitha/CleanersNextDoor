using FluentValidation;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator
      : AbstractValidator<CreateOrderCommand>
    {
        private readonly ICleanersNextDoorContext _context;
        public CreateOrderCommandValidator(ICleanersNextDoorContext context)
        {
            _context = context;
            RuleFor(v => v.MerchantID)
                .NotEmpty()
                .MustAsync(MerchantIsOpen)
                    .WithMessage("The specified username already exists.");
        }
        public async Task<bool> MerchantIsOpen(int merchantId,
            CancellationToken cancellationToken)
        {
            return await _context.Merchants
                .SingleOrDefaultAsync(m => m.ID == merchantId && m.Active) != null;
        }
    }
}
