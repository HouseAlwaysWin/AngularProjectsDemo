using System.Collections.Generic;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Models.Dtos
{
    public class UpdateBasketItemDto
    {
        public string BasketId { get; set; }
        public BasketItem BasketItem { get; set; } 
    }
}