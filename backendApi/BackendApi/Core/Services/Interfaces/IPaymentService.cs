using System.Threading.Tasks;
using BackendApi.Core.Models.Entities;

namespace BackendApi.Core.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<Basket> CreateOrUpdatePaymentIntent(string basketId);

        Task<Order> UpdateOrderPaymentSuccessed(string paymentIntentId);

        Task<Order> UpdateOrderPaymentFailed(string paymentIntentId);
    }
}