using System;
using System.Collections.Generic;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Models.Dtos
{
    public class OrderByIdDto
    {
         public int Id  { get; set; }
        public string BuyerName  { get; set; }
        public string BuyerEmail { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal SubTotalPrice {get;set;}
        public DateTimeOffset OrderDate { get; set; } 
        public OrderAddress ShipAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItemDto> OrderItems { get; set; }
        public string OrderStatus { get; set; } 
    }
}