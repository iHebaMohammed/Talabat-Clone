using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Helpers
{
    public class OrderItemPictureUrlResorver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResorver(IConfiguration configuration)
        {
            _configuration=configuration;
        }

        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            if (!source.Product.ProductImageUrl.IsNullOrEmpty())
            {
                return $"{_configuration["BaseApiUrl"]}/{source.Product.ProductImageUrl}";
            }
            return null;
        }
    }
}
