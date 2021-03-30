using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApi.Core.Data.QuerySpecs;
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

        

        public async Task<Order> CreateOrderAsync(string buyerEmail,string buyerName, int deliveryMethodId, string baseketId, OrderAddress address)
        {
             var basket = await _basketRepo.GetBasketAsync(baseketId);

             var items = new List<OrderItem>();
             foreach (var item in basket.BasketItems)
             {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var orderItem = new OrderItem(productItem.Id,item.Quantity);
                items.Add(orderItem);
             }

             var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

             var subTotal = items.Sum(item => item.ProductInfo.Price * item.Quantity);

             var spec = new GetOrderPaymentIntentIdSpec(basket.PaymentIntentId);
            //  var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            //  if(order != null){
            //      _unitOfWork.Repository<Order>().Delete(order);
            //  }

            var order = new Order(
                items,
                buyerName,
                buyerEmail,
                address,
                deliveryMethodId,
                subTotal,
                basket.PaymentIntentId
            );

            _unitOfWork.Repository<Order>().Add(order);

            var result = await _unitOfWork.Complete();

            if(result <=0) return null;
             
             return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            var result = await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
            return result;
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new GetOrderWithItemsSpec(id,buyerEmail);
            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new GetOrderWithItemsSpec(buyerEmail);
            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }
    }
}