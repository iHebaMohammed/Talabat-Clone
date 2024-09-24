using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class ProductItemOrdered
    {
        public ProductItemOrdered() { }
        public ProductItemOrdered(string productName, string productImageUrl, int productId)
        {
            ProductName=productName;
            ProductImageUrl=productImageUrl;
            ProductId=productId;
        }

        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }
        public int ProductId { get; set; }
    }
}
