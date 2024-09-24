﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications
{
    public class OrderSpecification : BaseSpecification<Order>
    {
        public OrderSpecification(string buyerEmail) : base(O => O.BuyerEmail == buyerEmail)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.ShippingAddress);
            Includes.Add(O => O.Items);
            AddOrderByDescending(O => O.OrderDate);
        }

        public OrderSpecification(int orderId , string buyerEmail) : base
            (
            O => O.Id == orderId && O.BuyerEmail == buyerEmail
            )
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.ShippingAddress);
            Includes.Add(O => O.Items);
        }
    }
}
