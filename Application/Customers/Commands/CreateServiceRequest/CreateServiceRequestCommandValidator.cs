using Domain.Entities;
using FluentValidation;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Commands.CreateServiceRequest
{
    public class CreateServiceRequestCommandValidator : AbstractValidator<CreateServiceRequestCommand>
    {
        private readonly ICleanersNextDoorContext _context;
        public CreateServiceRequestCommandValidator(ICleanersNextDoorContext context)
        {
            _context = context;

            RuleFor(v => v.CustomerID)
                .NotEmpty()
                .MustAsync(OrderBelongsToCustomer)
                    .WithMessage("The order must belong to the customer.");

            RuleFor(v => v.OrderID)
                .NotEmpty()
                .MustAsync(OrderHasLineItems)
                    .WithMessage("The order must have at least one line item.");

            RuleFor(v => v.OrderID)
                .NotEmpty()
                .MustAsync(OrderHasServiceRequest)
                    .WithMessage("The order must have at least one service request.");

            RuleFor(v => v.OrderID)
                .NotEmpty()
                .MustAsync(ValidateWorkflowCorrespondence)
                    .WithMessage("The merchant must indicate a valid workflow for creating correspondence steps.");

            //TODO: add order over payment check
        }

        /// <summary>
        /// validates that the order belongs to the current customer
        /// </summary>
        /// <param name="args"></param>
        /// <param name="customerId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> OrderBelongsToCustomer(CreateServiceRequestCommand args, int customerId,
            CancellationToken cancellationToken)
        {
            return await _context.Orders
                    .FirstOrDefaultAsync(o => o.ID == args.OrderID && o.CustomerID == customerId) != null;
        }

        /// <summary>
        /// validates the order has line items
        /// </summary>
        /// <param name="args"></param>
        /// <param name="orderId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> OrderHasLineItems(CreateServiceRequestCommand args, int orderId,
            CancellationToken cancellationToken)
        {
            return await _context.LineItems
                .AnyAsync(l => l.OrderID == args.OrderID);
        }

        /// <summary>
        /// validates the order has a service request attached
        /// </summary>
        /// <param name="args"></param>
        /// <param name="orderId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> OrderHasServiceRequest(CreateServiceRequestCommand args, int orderId,
            CancellationToken cancellationToken)
        {
            await Task.FromResult(0);
            return args.ServiceRequest != null; //await _context.ServiceRequests.AnyAsync(l => l.OrderID == args.OrderID);
        }
        /// <summary>
        /// validates the merchant that the order is for has the selected workflow
        /// and has provided enough correspondence addresses to complete the workflow item sequence
        /// </summary>
        /// <param name="args"></param>
        /// <param name="orderId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> ValidateWorkflowCorrespondence(CreateServiceRequestCommand args, int orderId,
            CancellationToken cancellationToken)
        {
            var data = from o in _context.Orders.AsEnumerable()
                       join mw in _context.MerchantWorkflows.AsEnumerable() on new { args.ServiceRequest.WorkflowID, o.MerchantID } equals new { mw.WorkflowID, mw.MerchantID }
                       join ws in _context.WorkflowSteps.AsEnumerable() on mw.WorkflowID equals ws.WorkflowID
                       join ct in _context.CorrespondenceTypes.AsEnumerable() on ws.CorrespondenceTypeID equals ct.ID
                       where o.ID == args.OrderID
                       select ws;
            if (data == null || data.FirstOrDefault() == null) 
                return false; //could not find workflow

            var workflowItems = data.Where(i => i.CorrespondenceType.CustomerConfigures || i.CorrespondenceType.MerchantConfigures).ToList();

            if(workflowItems.Count != args.CorrespondenceAddresses.Count)
                return false;   //invalid amount of addresses passes

            await Task.FromResult(0);
            return true;

        }
    }
}
