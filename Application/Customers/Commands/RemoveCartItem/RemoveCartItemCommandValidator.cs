using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Commands.RemoveCartItem
{
    public class RemoveCartItemCommandValidator : AbstractValidator<RemoveCartItemCommand>
    {
        private readonly IApplicationDbContext _context;
        public RemoveCartItemCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.CustomerID)
                .NotEmpty()
                .MustAsync(BeOpenOrder)
                    .WithMessage("The order must belong to the customer.");
        }

        public async Task<bool> BeOpenOrder(RemoveCartItemCommand args, int customerId,
            CancellationToken cancellationToken)
        {
            var data = from o in _context.Orders.AsEnumerable()
                       join ost in _context.OrderStatusTypes.AsEnumerable() on o.OrderStatusTypeID equals ost.ID
                       where o.ID == args.OrderID && o.CustomerID == args.CustomerID && o.IsOpenOrder()
                       select o;
            if (data == null || data.FirstOrDefault() == null) return false;

            await Task.FromResult(0);
            return data.First().ID > 0;
        }
    }
}
