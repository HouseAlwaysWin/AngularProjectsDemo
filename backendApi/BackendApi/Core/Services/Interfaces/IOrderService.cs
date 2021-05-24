using System.Collections.Generic;
using System.Threading.Tasks;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities;

namespace BackendApi.Core.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail,string buyerName, int deliveryMethodId, string baseketId, OrderAddress address) ;

        Task<List<Order>> GetOrdersForUserAsync(string buyerEmail);

        Task<Order> GetOrderByIdAsync(int id,string buyerEmail);
        Task<List<Order>> GetOrderByEmailListSpec(GetOrderParam param);

        Task<List<DeliveryMethodDto>> GetDeliveryMethodAsync();

    }
}