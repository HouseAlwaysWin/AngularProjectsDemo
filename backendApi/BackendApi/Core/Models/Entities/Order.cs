using System;
using System.Collections.Generic;


namespace BackendApi.Core.Models.Entities
{
    public class Order : IBaseEntity
    {
        public Order()
        {
            
        }
        public Order(
            IReadOnlyList<OrderItem> orderItems,
            string buyerName,
            string buyerEmail,
            OrderAddress orderAddress,
            int deliveryMethodId,
            decimal totalPrice,
            string paymentIntentId
           )
        {
            OrderItems = orderItems;
            BuyerName = buyerName;
            BuyerEmail = buyerEmail;
            TotalPrice = totalPrice;
            OrderAddress = orderAddress;
            DeliveryMethodId = deliveryMethodId;
            PaymentIntentId = paymentIntentId;
        }
        public string BuyerName  { get; set; }
        public string BuyerEmail { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderAddress OrderAddress { get; set; }
        public int OrderAddressId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public int DeliveryMethodId { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public string PaymentIntentId { get; set; }
        public int Id { get ; set ; }
        public DateTimeOffset CreatedDate { get ; set ; }
        public DateTimeOffset? ModifiedDate { get ; set ; }
    }
}