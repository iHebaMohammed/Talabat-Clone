using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.DTOs
{
    public class OrderToReturnDTO
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; }
        public Address ShippingAddress { get; set; }
        //public DeliveryMethod DeliveryMethod { get; set; } // Navigational Property [One]

        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDTO> Items { get; set; } // Navigational Property [Many]
        public string PaymentIntentId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
    }
}
