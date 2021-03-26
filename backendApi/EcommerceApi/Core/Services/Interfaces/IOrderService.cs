using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail,int deliveryMethod,string basektId,OrderAddress address) ;

        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);

        Task<Order> GetOrderByIdAsync(int id,string buyerEmail);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync();

    }
}