using System;
using System.Collections.Generic;


namespace EcommerceApi.Core.Models.Entities
{
    public class Order : BaseEntity
    {
        public Order()
        {
            
        }
        public Order(
            IReadOnlyList<OrderItem> orderItems,
            string buyerName,
            string buyerEmail,
            OrderAddress shipAddress,
            DeliveryMethod deliveryMethod,
            decimal totalPrice,
            string paymentIntentId
           )
        {
            OrderItems = orderItems;
            BuyerName = buyerName;
            BuyerEmail = buyerEmail;
            TotalPrice = totalPrice;
            ShipAddress = shipAddress;
            DeliveryMethod = deliveryMethod;
            PaymentIntentId = paymentIntentId;
        }
        public int Id  { get; set; }
        public string BuyerName  { get; set; }
        public string BuyerEmail { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderAddress ShipAddress { get; set; }
        public int ShipAddressId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public string PaymentIntentId { get; set; }
    }
}