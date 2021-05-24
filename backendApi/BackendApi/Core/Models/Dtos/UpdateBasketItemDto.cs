using System.Collections.Generic;
using BackendApi.Core.Models.Entities;

namespace BackendApi.Core.Models.Dtos
{
    public class UpdateBasketItemDto
    {
        public string BasketId { get; set; }
        public BasketItem BasketItem { get; set; } 
    }
}