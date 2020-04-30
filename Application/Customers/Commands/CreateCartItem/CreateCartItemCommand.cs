using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Commands.CreateCartItem
{
    public class CreateCartItemCommand : IRequest<CreateCartItemModel>
    {
        public int CustomerID { get; set; }
        public int ItemID { get; set; }
        public int? OrderID { get; set; }
        public int? NewQty { get; set; }
        public bool AddSingleItem => NewQty == null;
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

        public CreateCartItemCommandHandler(
            ICleanersNextDoorContext context
            )
        {
            _context = context;
        }
        public async Task<CreateCartItemModel> Handle(CreateCartItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _context.Items.FindAsync(request.ItemID);

            #region Create new order if needed
            if (request.OrderID == null || request.OrderID == 0)
            {
                var open = _context.OrderStatusTypes.First(t => t.Name.ToUpper() == "OPEN");
                var order = new Order
                {
                    CustomerID = request.CustomerID,
                    Active = true,
                    CreatedAt = DateTime.Now,
                    MerchantID = item.MerchantID,
                    OrderStatusTypeID = open.ID,
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync(cancellationToken);
                request.OrderID = order.ID;
            }

            if (request.OrderID == 0) return new CreateCartItemModel();
            #endregion

            var lineItemAmount = 0M;

            #region Determine Price
            switch (item.PriceTypeID)
            {
                default:
                    lineItemAmount = item.Price.Value;
                    break;
            }
            #endregion

            try
            {
                var lineItems = _context.LineItems
                    .Where(l => l.OrderID == request.OrderID && l.ItemID == request.ItemID)
                    .ToList();

                var currentQty = lineItems.Count;

                if (request.AddSingleItem)
                {
                    //add single item
                    _context.LineItems.Add(new LineItem
                    {
                        OrderID = request.OrderID.Value,
                        ItemID = request.ItemID,
                        ItemAmount = lineItemAmount
                    });
                }
                else if (currentQty > 0 && request.NewQty == 0)
                {
                    //remove all items
                    var removeAllLineItems = _context.LineItems.Where(l => l.OrderID == request.OrderID && l.ItemID == request.ItemID);
                    _context.LineItems.RemoveRange(removeAllLineItems);
                }
                else if (request.NewQty > 0)
                {
                    var qtyDiff = request.NewQty.Value - currentQty;
                    if (qtyDiff > 0)
                    {
                        for (int i = 0; i < qtyDiff; i++)
                            _context.LineItems.Add(new LineItem
                            {
                                OrderID = request.OrderID.Value,
                                ItemID = request.ItemID,
                                ItemAmount = lineItemAmount
                            });

                    }
                    else if (qtyDiff < 0)
                    {
                        qtyDiff = -qtyDiff;

                        lineItems = lineItems != null && qtyDiff < lineItems.Count
                            ? lineItems.GetRange(lineItems.Count - qtyDiff, qtyDiff)
                            : lineItems;

                        _context.LineItems.RemoveRange(lineItems);
                    }
                    else
                    {
                        //do nothing, the current qty is equal to the NewQty
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);

                return new CreateCartItemModel
                {
                    ItemID = request.ItemID,
                    OrderID = request.OrderID.Value,
                    NewQty = null
                };
            }
            catch (Exception e)
            {
                return new CreateCartItemModel();
            }
        }
    }
}
