using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.IServices;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class OrderController : BaseApiController
    {
        private readonly IOrderServices _orderServices;
        private readonly IMapper _mapper;

        public OrderController(IOrderServices orderServices , IMapper mapper)
        {
            _orderServices = orderServices;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDTO)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var address = _mapper.Map<AddressDTO , Address>(orderDTO.ShippingAddress);
            var order = await _orderServices.CreateOrderAsync(buyerEmail, orderDTO.BasketId , orderDTO.DeliveryMethodId ,address);

            if (order == null) 
                return BadRequest(new ApisResponse(400));

            var orderMapped = _mapper.Map<Order, OrderToReturnDTO>(order);
            return Ok(orderMapped);
        }

        [CachedAttribute(600)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderServices.GetOrdersForUserAsync(buyerEmail);

            var orderMapped = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDTO>>(orders);
            return Ok(orderMapped);
        }

        [CachedAttribute(600)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderForUser(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderServices.GetOrderByIdForUserAsync(id, buyerEmail);
            if (order == null)
                return BadRequest(new ApisResponse(400));

            var orderMapped = _mapper.Map<Order, OrderToReturnDTO>(order);
            return Ok(orderMapped);
        }

        [CachedAttribute(600)]
        [HttpGet("DeliveryMethod")]
        public async Task<ActionResult<DeliveryMethod>> GetDeliveryMethods()
        {
            var deliveryMethods = await _orderServices.GetDeliveryMethodsAsync();
            return Ok(deliveryMethods);
        }

    }
}
