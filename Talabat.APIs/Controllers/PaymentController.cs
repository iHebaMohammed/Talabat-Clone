using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.IServices;

namespace Talabat.APIs.Controllers
{
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private const string _wbSecret = "whsec_412e4a57256ec29341d26a225343fce45c03b0c482d167b524c2251256a37be9";


        public PaymentController(IPaymentService paymentService , IMapper mapper , ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _mapper=mapper;
            _logger=logger;
        }

        [HttpPost("basketId")]
        [Authorize]
        public async Task<ActionResult<CustomerBasketDTO>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket == null)
                return BadRequest(new ApisResponse(400 , "Problem with your basket"));
            
            var mappedBasket = _mapper.Map<CustomerBasket, CustomerBasketDTO>(basket);

            return Ok(mappedBasket);
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _wbSecret);

            PaymentIntent intent;
            Order order;

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Succeded");
                    order = await _paymentService.UpdateOrderPaymentSucceeded(intent.Id);
                    break;
                case "payment_intent.payment_failed":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Faild", intent.Id);
                    order = await _paymentService.UpdateOrderPaymentFalid(intent.Id);
                    _logger.LogInformation("Payment Faild", order.Id);
                    break;
            }
            return new EmptyResult();
        }

    }
}
