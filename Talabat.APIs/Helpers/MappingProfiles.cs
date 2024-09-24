using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(D => D.ProductBrand, O => O.MapFrom(S => S.ProductBrand.Name))
                .ForMember(D => D.ProductType, O => O.MapFrom(S => S.ProductType.Name))
                .ForMember(D => D.PictureUrl , O => O.MapFrom<ProductPictureUrlResolver>());

            CreateMap<Core.Entities.Identity.Address , AddressDTO>().ReverseMap();
            CreateMap<Core.Entities.Order_Aggregate.Address, AddressDTO>().ReverseMap();
            CreateMap<CustomerBasket , CustomerBasketDTO>().ReverseMap();
            CreateMap<BasketItem, BasketItemDTO>().ReverseMap();

            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(D => D.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(D => D.DeliveryMethodCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));
            //.ReverseMap();

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(D => D.ProductId, O => O.MapFrom(S => S.Product.ProductId))
                .ForMember(D => D.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                .ForMember(D => D.ProductImageUrl, O => O.MapFrom(S => S.Product.ProductImageUrl))
                .ForMember(D => D.ProductImageUrl, O => O.MapFrom<OrderItemPictureUrlResorver>());
        }
    }
}
