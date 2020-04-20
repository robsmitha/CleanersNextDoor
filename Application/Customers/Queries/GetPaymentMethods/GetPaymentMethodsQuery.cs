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

namespace Application.Customers.Queries.GetPaymentMethods
{
    public class GetPaymentMethodsQuery : IRequest<List<PaymentMethodModel>>
    {
        public int CustomerID { get; set; }
        public GetPaymentMethodsQuery(int customerId)
        {
            CustomerID = customerId;
        }
    }

    public class GetPaymentMethodsQueryHandler : IRequestHandler<GetPaymentMethodsQuery, List<PaymentMethodModel>>
    {
        private readonly ICleanersNextDoorContext _context;
        private IMapper _mapper;

        public GetPaymentMethodsQueryHandler(
            ICleanersNextDoorContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<PaymentMethodModel>> Handle(GetPaymentMethodsQuery request, CancellationToken cancellationToken)
        {
            var paymentMethods = await _context.PaymentMethods
                .Where(p => p.CustomerID == request.CustomerID)
                .ToListAsync();
            return _mapper.Map<List<PaymentMethodModel>>(paymentMethods);
        }
    }

}
