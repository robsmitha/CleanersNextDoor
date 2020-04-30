using Application.Merchants.Queries.GetMerchantWorkflow;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Merchants.Queries.GetMerchantWorkflow
{
    public class GetMerchantWorkflowQuery : IRequest<GetMerchantWorkflowModel>
    {
        public int MerchantID { get; set; }
        public int CustomerID { get; set; }
        public GetMerchantWorkflowQuery(int merchantId, int customerId)
        {
            MerchantID = merchantId;
            CustomerID = customerId;
        }
    }
    public class GetWorkflowQueryHandler : IRequestHandler<GetMerchantWorkflowQuery, GetMerchantWorkflowModel>
    {
        private readonly ICleanersNextDoorContext _context;
        private readonly IMapper _mapper;
        public GetWorkflowQueryHandler(ICleanersNextDoorContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<GetMerchantWorkflowModel> Handle(GetMerchantWorkflowQuery request, CancellationToken cancellationToken)
        {
            var data = from mw in _context.MerchantWorkflows.AsEnumerable()
                       join m in _context.Merchants.AsEnumerable() on mw.MerchantID equals m.ID
                       join w in _context.Workflows.AsEnumerable() on mw.WorkflowID equals w.ID
                       join ws in _context.WorkflowSteps.AsEnumerable() on mw.WorkflowID equals ws.WorkflowID
                       join ct in _context.CorrespondenceTypes.AsEnumerable() on ws.CorrespondenceTypeID equals ct.ID
                       join c in _context.Customers.AsEnumerable() on request.CustomerID equals c.ID
                       join ml in _context.MerchantLocations.AsEnumerable() on new { request.MerchantID, ws.CorrespondenceTypeID, IsDefault = true } equals new { ml.MerchantID, ml.CorrespondenceTypeID, ml.IsDefault } into tmp_ml 
                       from ml in tmp_ml.DefaultIfEmpty()
                       join ca in _context.CustomerAddresses.AsEnumerable() on new { request.CustomerID, ws.CorrespondenceTypeID, IsDefault = true } equals new { ca.CustomerID, ca.CorrespondenceTypeID, ca.IsDefault} into tmp_ca
                       from ca in tmp_ca.DefaultIfEmpty()
                       where mw.MerchantID == request.MerchantID && mw.IsDefault
                       select new { mw, ws, c, ca, ml };

            if (data == null || data.FirstOrDefault() == null) return new GetMerchantWorkflowModel();
            
            var model = _mapper.Map<GetMerchantWorkflowModel>(data.First().mw);
            model.Customer = data.First().c != null
                ? _mapper.Map<WorkflowCustomerModel>(data.First().c)
                : new WorkflowCustomerModel();
            foreach (var m in data.OrderBy(s => s.ws.Step))
            {
                var step = _mapper.Map<WorkflowStepModel>(m.ws);
                var address = new WorkflowStepAddressModel();

                if (m.ml != null)
                {
                    step.Address = _mapper.Map<WorkflowStepAddressModel>(m.ml);
                }
                else if (m.ca != null)
                {
                    step.Address = _mapper.Map<WorkflowStepAddressModel>(m.ca);
                }
                //TODO: calculate default scheduled at time based on correspondence time, steps, etc.
                step.ScheduledAt = DateTime.Now;
                model.Steps.Add(step);
            }

            await Task.FromResult(0);
            return model;
        }
    }
}
