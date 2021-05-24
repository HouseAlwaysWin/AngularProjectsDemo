using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendApi.Core.Data.QuerySpecs;
using BackendApi.Core.Data.Repositories;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Data.Services.Interfaces;
using BackendApi.Core.Models.Entities;
using BackendApi.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace BackendApi.Core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketService _basketService;
        private readonly IStoreRepository _storeRepo;
        private readonly IConfiguration _config;
        private readonly string  _env;

        public PaymentService(
            IBasketService basketService,
            IStoreRepository storeRepo,
            IConfiguration config
        )
        {
            this._basketService = basketService;
            this._storeRepo = storeRepo;
            this._config = config;
            _env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }
        public async Task<Basket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = (_env =="Development")? _config["StripeSettings:SecretKey"]:Environment.GetEnvironmentVariable("StripeSettings:SecretKey");

            var basket = await _basketService.GetBasketAsync(basketId);

            if(basket == null) return null;

            decimal shippingPrice = 0m;

            if(basket.DeliveryMethodId.HasValue){
                // var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>()
                //             .GetByIdAsync((int)basket.DeliveryMethodId);
                var deliveryMethod = await _storeRepo.GetByIdAsync<DeliveryMethod>((int)basket.DeliveryMethodId);
                shippingPrice = deliveryMethod.Price;
            }

            // foreach(var item in basket.BasketItems){
            //     // var productItem = await _unitOfWork.Repository<Models.Entities.Product>().GetByIdAsync(item.Id);
            //     var productItem = await _unitOfWork.EntityRepository<Models.Entities.Product>().GetByIdAsync(item.Id);
            //     if(item.Price != productItem.Price){
            //         item.Price = productItem.Price;
            //     }
            // }

            var service = new PaymentIntentService();

            PaymentIntent intent;

            if(string.IsNullOrEmpty(basket.PaymentIntentId)){
                var options = new PaymentIntentCreateOptions{
                    // Because price is decimal so we need multiplue 100 to convert to long type.
                    Amount = (long) basket.BasketItems.Sum(i => i.Quantity *  i.Price *100 ) + (long)shippingPrice,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>{"card"}
                };
                intent =await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else{
                var options = new PaymentIntentUpdateOptions{
                    // Because price is decimal so we need multiplue 100 to convert to long type.
                    Amount =(long) basket.BasketItems.Sum(i => i.Quantity * i.Price *100) + (long)shippingPrice 
                };
            }

            await _basketService.UpdateBasketAsync(basket);

            return basket;
        }

        public async Task<Models.Entities.Order> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            // var spec = new OrderByPaymentIntentIdSpec(paymentIntentId);
            var order = await _storeRepo.GetByAsync<Models.Entities.Order>(q => {
                return q.Where(o => o.PaymentIntentId == paymentIntentId);
            });
            if(order == null) return null;

            order.OrderStatus = OrderStatus.PaymentFailed;
            // _unitOfWork.Repository<Models.Entities.Order>().Update(order);

            _storeRepo.Update(order);
            await _storeRepo.CompleteAsync();
            return null;
        }

        public async Task<Models.Entities.Order> UpdateOrderPaymentSuccessed(string paymentIntentId)
        {
            //  var spec = new OrderByPaymentIntentIdSpec(paymentIntentId);
            // var order = await _unitOfWork.Repository<Models.Entities.Order>().GetEntityWithSpec(spec);
            var order = await _storeRepo.GetByAsync<Models.Entities.Order>(q => {
                return q.Where(o => o.PaymentIntentId == paymentIntentId);
            });

            if(order == null) return null;

            order.OrderStatus = OrderStatus.PaymentReceived;

            await _storeRepo.CompleteAsync();
            return order;
        }
    }
}