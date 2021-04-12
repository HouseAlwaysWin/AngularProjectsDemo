using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public OrderService(
            IBasketRepository basketRepo,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._basketRepo = basketRepo;
            this._mapper = mapper;
        }

        

        public async Task<Order> CreateOrderAsync(string buyerEmail,string buyerName, int deliveryMethodId, string baseketId, OrderAddress address)
        {
             var basket = await _basketRepo.GetBasketAsync(baseketId);

             var items = new List<OrderItem>();
             decimal subTotal = 0m;
             foreach (var item in basket.BasketItems)
             {
                var productSpec = new ProductIncludeAllSpec(item.Id);
                var productItem = await _unitOfWork.Repository<Product>().GetEntityWithSpec(productSpec);
                subTotal += (productItem.Price * item.Quantity);

                var orderItem =  _mapper.Map<BasketItem,OrderItem>(item);
                // orderItem.ProductBrand = productItem.ProductBrand.Name;
                orderItem.ProductCategory = productItem.ProductCategory.Name;
                items.Add(orderItem);
             }

             var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);


             var spec = new GetOrderPaymentIntentIdSpec(basket.PaymentIntentId);

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

        public async Task<IReadOnlyList<Order>> GetOrderByEmailListSpec(GetOrderParam param)
        {
            var spec = new GetOrderByEmailListSpec(param);
            return await _unitOfWork.Repository<Order>().ListAsync(spec);
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