using Application.LineItems;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Queries.GetCustomerCart
{
    public class GetCustomerCartQuery : IRequest<CustomerCartModel>
    {
        public int CustomerID { get; set; }
        public int MerchantID { get; set; }
        public GetCustomerCartQuery(int customerId, int merchantId)
        {
            CustomerID = customerId;
            MerchantID = merchantId;
        }
    }
    public class GetCustomerCartQueryHandler : IRequestHandler<GetCustomerCartQuery, CustomerCartModel>
    {
        private readonly ICleanersNextDoorContext _context;
        private IMapper _mapper;

        public GetCustomerCartQueryHandler(
            ICleanersNextDoorContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerCartModel> Handle(GetCustomerCartQuery request, CancellationToken cancellationToken)
        {
            var lineItems = await _context.LineItems
                .Include(o => o.Order)
                .Include(i => i.Item)
                .Where(l => l.Order.CustomerID == request.CustomerID && l.Order.MerchantID == request.MerchantID)
                .ToListAsync();

            if (lineItems == null || lineItems.Count() == 0) return new CustomerCartModel();

            var distinctItems = lineItems
                .GroupBy(x => x.ItemID)
                .Select(g => g.First())
                .ToList();

            var items = _mapper.Map<IEnumerable<LineItemModel>>(distinctItems);

            var results = new List<CustomerCartItemModel>();
            foreach (var item in items)
            {
                var qty = lineItems
                        .Where(x => x.ItemID == item.ItemID)
                        .Count();

                results.Add(new CustomerCartItemModel(item, qty));
            }

            var displayPrice = lineItems.Sum(x => x.ItemAmount).ToString("C");
            return new CustomerCartModel(results, displayPrice);
        }
    }
}
