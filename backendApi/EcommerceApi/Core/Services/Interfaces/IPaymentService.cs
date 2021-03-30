using System.Threading.Tasks;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<Basket> CreateOrUpdatePaymentIntent(string basketId);

        Task<Order> UpdateOrderPaymentSuccessed(string paymentIntentId);

        Task<Order> UpdateOrderPaymentFailed(string paymentIntentId);
    }
}