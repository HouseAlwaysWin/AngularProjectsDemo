using System.Collections.Generic;
using BackendApi.Core.Models.Entities;

namespace BackendApi.Core.Models.Dtos
{
    public class OrderDto
    {
         public long Id  { get; set; }
        public string BuyerName  { get; set; }
        public string BuyerEmail { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderDate { get; set; } 
        public OrderAddress OrderAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItemDto> OrderItems { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentIntentId { get; set; }
        public string CreatedDate { get; set; } 
    }
}