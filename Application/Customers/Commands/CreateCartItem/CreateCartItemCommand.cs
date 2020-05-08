using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Commands.CreateCartItem
{
    public class CreateCartItemCommand : IRequest<CreateCartItemModel>
    {
        public int CustomerID { get; set; }
        public int ItemID { get; set; }
        public int OrderID { get; set; }
        public int NewQty { get; set; }
        public CreateCartItemCommand(CreateCartItemModel model, int customerId)
        {
            ItemID = model.ItemID;
            OrderID = model.OrderID;
            NewQty = model.NewQty;
            CustomerID = customerId;
        }
    }
    public class CreateCartItemCommandHandler : IRequestHandler<CreateCartItemCommand, CreateCartItemModel>
    {
        private readonly ICleanersNextDoorContext _context;

        public CreateCartItemCommandHandler(ICleanersNextDoorContext context)
        {
            _context = context;
        }

        public async Task<CreateCartItemModel> Handle(CreateCartItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _context.Items.FindAsync(request.ItemID);
            var data = await Task.FromResult(from o in _context.Orders.AsEnumerable()
                                             join m in _context.Merchants.AsEnumerable() on o.MerchantID equals m.ID
                                             join ost in _context.OrderStatusTypes.AsEnumerable() on o.OrderStatusTypeID equals ost.ID
                                             join li in _context.LineItems.AsEnumerable() on o.ID equals li.OrderID into tmp_li
                                             from li in tmp_li.DefaultIfEmpty()
                                             join i in _context.Items.AsEnumerable() on li?.ItemID equals i.ID into tmp_i
                                             from i in tmp_i.DefaultIfEmpty()
                                             where o.CustomerID == request.CustomerID
                                             && o.MerchantID == item.MerchantID
                                             && o.IsOpenOrder()
                                             select new { o, li });

            var rows = data?.ToList();
            Order order = null;
            List<LineItem> itemLineItems = new List<LineItem>();
            if(rows != null)
            {
                foreach (var row in rows)
                {
                    if (order == null && row.o != null)
                        order = row.o;

                    if (order != null && row.li != null && order.ID == row.li.OrderID)
                        itemLineItems.Add(row.li);
                }
            }

            if (order == null)
            {
                // Create new order if needed
                var open = from ost in _context.OrderStatusTypes.AsUntrackedEnumerable()
                           where ost.IsOpenOrderStatus()
                           select ost;

                order = new Order
                {
                    CustomerID = request.CustomerID,
                    Active = true,
                    CreatedAt = DateTime.Now,
                    MerchantID = item.MerchantID,
                    OrderStatusTypeID = open.First().ID,
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var itemCount = itemLineItems.Count;
            var newQty = itemCount + request.NewQty;

            //handle qty difference
            var qtyDiff = newQty - itemCount;
            if(qtyDiff == 0)
            {
                //do nothing, the current qty is equal to the NewQty
            }
            else
            {
                if (qtyDiff > 0)
                {
                    for (int i = 0; i < qtyDiff; i++)
                        _context.LineItems.Add(new LineItem
                        {
                            OrderID = order.ID,
                            ItemID = item.ID,
                            ItemAmount = item.Price ?? 0M   //todo: add amount to request for variable cost items
                        });
                }
                else if (qtyDiff < 0)
                {
                    qtyDiff = -qtyDiff;

                    var removelineItems = qtyDiff < itemCount
                        ? itemLineItems.GetRange(itemCount - qtyDiff, qtyDiff)
                        : itemLineItems;

                    _context.LineItems.RemoveRange(removelineItems);
                }
                await _context.SaveChangesAsync(cancellationToken);
            }

            var response = new CreateCartItemModel(order.ID);
            return response;
        }
    }
}
