using System.Collections.Generic;

namespace BackendApi.Core.Models.Entities
{
    public class Basket
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