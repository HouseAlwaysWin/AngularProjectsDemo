using System.Collections.Generic;

namespace EcommerceApi.Core.Models.Entities
{
    public class Basket:BaseEntity
    {
        public Basket()
        {
        }

        public Basket(string id)
        {
           this.Id = id; 
        }

        public List<BasketItem> BasketItems {get;set;} = new List<BasketItem>();

        public string Id { get; set; }
        public int? DeliveryMethodId { get; set; }
        public string ClientSecret { get; set; }
        public decimal ShippingPrice { get; set; }
        public string PaymentIntentId { get; set; }
    }
}