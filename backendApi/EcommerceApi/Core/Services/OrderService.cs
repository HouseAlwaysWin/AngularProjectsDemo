using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApi.Core.Data.Repositories.Interfaces;
using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Entities;
using EcommerceApi.Core.Services.Interfaces;

namespace EcommerceApi.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(
            IBasketRepository basketRepo,
            IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._basketRepo = basketRepo;
            
        }

        public Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethod, string basektId, OrderAddress address)
        {
            throw new System.NotImplementedException();
        }

        // public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string baseketId, OrderAddress address)
        // {
        //      var basket = await _basketRepo.GetBasketAsync(baseketId);

        //      var items = new List<OrderItem>();
        //      foreach (var item in basket.BasketItems)
        //      {
        //         var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
        //         var orderItem = new OrderItem(productItem.Id,item.Quantity,productItem);
        //         items.Add(orderItem);
        //      }

        //      var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

        //      var subTotal = items.Sum(item => item.ProductInfo.Price * item.Quantity);
        // }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new System.NotImplementedException();
        }
    }
}