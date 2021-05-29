using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BackendApi.Core.Data.QuerySpecs;
using BackendApi.Core.Data.Repositories;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Data.Services.Interfaces;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities;
using BackendApi.Core.Services.Interfaces;

namespace BackendApi.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketService _basketRepo;
        private readonly IStoreRepository _storeRepo;
        private readonly IMapper _mapper;
        private readonly ICachedService _cachedService;
        private readonly ILocalizedService _localizedService;

          private string _deliveryMethodKey { get { return $"{_cachedService.GetCurrentLang()}_all_deliveryMethod_key"; } }

        public OrderService(
            ILocalizedService localizedService,
            ICachedService cachedService,
            IBasketService basketRepo,
            IMapper mapper,
            IStoreRepository storeRepo)
        {
            this._storeRepo = storeRepo;
            this._basketRepo = basketRepo;
            this._mapper = mapper;
            this._cachedService = cachedService;
            this._localizedService = localizedService;
        }

       

        public async Task<Order> CreateOrderAsync(string buyerEmail,string buyerName, int deliveryMethodId, string baseketId, OrderAddress address)
        {
             var basket = await _basketRepo.GetBasketAsync(baseketId);

             var items = new List<OrderItem>();
             decimal subTotal = 0m;
             foreach (var item in basket.BasketItems)
             {
                // var productSpec = new ProductIncludeAllSpec(item.Id);
                 // var productItem = await _unitOfWork.Repository<Product>().GetEntityWithSpec(productSpec);
                // subTotal += (productItem.Price * item.Quantity);

                subTotal += (item.Price * item.Quantity);

                var orderItem =  _mapper.Map<BasketItem,OrderItem>(item);
                // orderItem.ProductCategory = productItem.ProductCategory.Name;
                items.Add(orderItem);
             }

             var deliveryMethod = await _storeRepo.GetByAsync<DeliveryMethod>(
                 query => query.Where(dm => dm.Id == deliveryMethodId));

             subTotal += deliveryMethod.Price;

            var order = new Order(
                items,
                buyerName,
                buyerEmail,
                address,
                deliveryMethodId,
                subTotal,
                basket.PaymentIntentId
            );

            await _storeRepo.AddAsync(order);
            try{
                var result = await _storeRepo.CompleteAsync();
                if(result) return null;
             
                return order;

            }
            catch(Exception ex){
                System.Console.WriteLine(ex);
            }

        return null;
    }

        public async Task<List<DeliveryMethodDto>> GetDeliveryMethodAsync()
        {
            return await _cachedService.GetAndSetAsync<List<DeliveryMethodDto>>(_deliveryMethodKey, async () =>
            {
               var result = await _storeRepo.GetAllAsync<DeliveryMethod>();
               foreach (var item in result)
                {
                    item.ShortName = await this._localizedService.GetLocalizedAsync(item,d => d.ShortName);
                    item.Description = await this._localizedService.GetLocalizedAsync(item,d => d.Description);
                    item.DeliveryTime = await this._localizedService.GetLocalizedAsync(item,d => d.DeliveryTime);
                }
                return _mapper.Map<List<DeliveryMethod>,List<DeliveryMethodDto>>(result.OrderBy(d => d.Id).ToList());
           });
        }

        public async Task<List<Order>> GetOrderByEmailListSpec(GetOrderParam param)
        {
            // var spec = new GetOrderByEmailListSpec(param);
            var result = await _storeRepo.GetAllAsync<Order>(q => OrderSpec.GetOrderByEmailListSpec(q,param));
            return result.ToList();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            // var spec = new GetOrderWithItemsSpec(id,buyerEmail);
            var result = await _storeRepo.GetByAsync<Order>(q => OrderSpec.GetOrderWithItemsSpec(q,buyerEmail));
            return result;
        }

        public async Task<List<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            // var spec = new GetOrderWithItemsSpec(buyerEmail);
            var result = await _storeRepo.GetAllAsync<Order>(q => OrderSpec.GetOrderWithItemsSpec(q,buyerEmail));
            return result.ToList();
        }
    }
}