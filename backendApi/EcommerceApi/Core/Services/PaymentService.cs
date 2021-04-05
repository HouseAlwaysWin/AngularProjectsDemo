using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApi.Core.Data.QuerySpecs;
using EcommerceApi.Core.Data.Repositories.Interfaces;
using EcommerceApi.Core.Models.Entities;
using EcommerceApi.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace EcommerceApi.Core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public PaymentService(
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IConfiguration config
        )
        {
            this._basketRepository = basketRepository;
            this._unitOfWork = unitOfWork;
            this._config = config;
        }
        public async Task<Basket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];

            var basket = await _basketRepository.GetBasketAsync(basketId);

            if(basket == null) return null;

            decimal shippingPrice = 0m;

            if(basket.DeliveryMethodId.HasValue){
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>()
                            .GetByIdAsync((int)basket.DeliveryMethodId);
                shippingPrice = deliveryMethod.Price;
            }

            foreach(var item in basket.BasketItems){
                var productItem = await _unitOfWork.Repository<Models.Entities.Product>().GetByIdAsync(item.Id);
                if(item.Price != productItem.Price){
                    item.Price = productItem.Price;
                }
            }

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

            await _basketRepository.UpdateBasketAsync(basket);

            return basket;
        }

        public async Task<Models.Entities.Order> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var spec = new OrderByPaymentIntentIdSpec(paymentIntentId);
            var order = await _unitOfWork.Repository<Models.Entities.Order>().GetEntityWithSpec(spec);
            if(order == null) return null;

            order.OrderStatus = OrderStatus.PaymentFailed;
            _unitOfWork.Repository<Models.Entities.Order>().Update(order);

            await _unitOfWork.Complete();
            return null;
        }

        public async Task<Models.Entities.Order> UpdateOrderPaymentSuccessed(string paymentIntentId)
        {
             var spec = new OrderByPaymentIntentIdSpec(paymentIntentId);
            var order = await _unitOfWork.Repository<Models.Entities.Order>().GetEntityWithSpec(spec);
            if(order == null) return null;

            order.OrderStatus = OrderStatus.PaymentReceived;

            await _unitOfWork.Complete();
            return order;
        }
    }
}