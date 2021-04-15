using System.Threading.Tasks;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Data.Services.Interfaces
{
    public interface IBasketService
    {
        Task<Basket> GetBasketAsync(string basketId);
        Task<Basket> UpdateBasketAsync(Basket basket);
        Task<Basket> UpdateBasketItemQuantityAsync(string basketId,BasketItem basketItem);
        Task<bool> RemoveBasketAsync(string basketId);
        Task<Basket> RemoveBasketItemAsync(string basketId,BasketItem basketItem);
    }
}