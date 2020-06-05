using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Customers.Commands.CreateServiceRequest
{
    public class PaymentModel
    {
        public string StripePaymentMethodID { get; set; }
        public long ChargedTimestamp { get; set; }
        public DateTime ChargedAt => new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)
            .AddSeconds(ChargedTimestamp)
            .ToLocalTime();
    }
}
