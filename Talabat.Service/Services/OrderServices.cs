using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.IRepositories;
using Talabat.Core.IServices;
using Talabat.Core.Specifications;

namespace Talabat.Service.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderServices(
            IBasketRepository basketRepository ,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            // 1. Get basket from basket repo
            var basket = await _basketRepository.GetBasketAsync(basketId);
            
            // 2. Get selected items at basket from products repos
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var productItemOrder = new ProductItemOrdered(product.Name, product.PictureUrl, product.Id);

                var orderItem = new OrderItem(productItemOrder, product.Price, item.Quantity);
                orderItems.Add(orderItem);
            }

            // 3. calculate sub total
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            // 4.Get delivery method from delivery method repo
            var deliveryMethod =await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            var orderByPaymentIntentIdSpecification = new OrderByPaymentIdSpecification(basket.PaymentIntentId);
            var existingOrder = await _unitOfWork.Repository<Order>().GetByIdWithSpecificationAsync(orderByPaymentIntentIdSpecification);

            if (existingOrder != null)
            {
                _unitOfWork.Repository<Order>().DeleteAsync(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }

            // 5. Create order
            var order = new Order(buyerEmail ,shippingAddress ,deliveryMethod , orderItems , subTotal , basket.PaymentIntentId);
            await _unitOfWork.Repository<Order>().CreateAsync(order);
            // 6. Save to database 

            var result = await _unitOfWork.Complete();
            if (result <= 0)
                return null;
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return deliveryMethods;
        }

        public async Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var specification = new OrderSpecification(orderId, buyerEmail);
            var order = await _unitOfWork.Repository<Order>().GetByIdWithSpecificationAsync(specification);
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var specification = new OrderSpecification(buyerEmail);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecificationAsync(specification);
            return orders;
        }
    }
}
