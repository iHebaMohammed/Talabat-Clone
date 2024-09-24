using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications
{
    public class OrderByPaymentIdSpecification : BaseSpecification<Order>
    {
        public OrderByPaymentIdSpecification(string paymentId) : base(O => O.PaymentIntentId == paymentId)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.ShippingAddress);
            Includes.Add(O => O.Items);
            AddOrderByDescending(O => O.OrderDate);
        }

    }
}
