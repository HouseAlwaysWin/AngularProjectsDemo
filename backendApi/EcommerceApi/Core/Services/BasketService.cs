using System.Runtime.CompilerServices;
using System.Linq;
using System;
using System.Threading.Tasks;
using EcommerceApi.Core.Data.Repositories.Interfaces;
using EcommerceApi.Core.Models.Entities;
using Newtonsoft.Json;
using StackExchange.Redis;
using AutoMapper;
using EcommerceApi.Core.Services.Interfaces;
using EcommerceApi.Core.Data.Services.Interfaces;

namespace EcommerceApi.Core.Services.Repositories
{
    public class BasketService : IBasketService
    {
        private readonly IDatabase _database;
        private readonly IRedisCachedService _redisCachedService;
        private readonly IMapper _mapper;

        public BasketService(
            IConnectionMultiplexer redis,
            IRedisCachedService  redisCachedService,
            IMapper mapper)
        {
            this._redisCachedService = redisCachedService;
            this._mapper = mapper;
            this._database = redis.GetDatabase();
        }

        public async Task<bool> RemoveBasketAsync(string basketId)
        {
            return await _redisCachedService.DeleteAsync(basketId);
        }

        public async Task<Basket> RemoveBasketItemAsync(string basketId,BasketItem basketItem){
            Basket basket = await _redisCachedService.GetAsync<Basket>(basketId);
            if(basket!=null){
                var item = basket.BasketItems.FirstOrDefault(b => b.Id == basketItem.Id);
                basket.BasketItems.Remove(item);
            }

            var update = await _redisCachedService.SetAsync<Basket>(basketId,basket);

            return basket;
        }

        public async Task<Basket> GetBasketAsync(string basketId)
        {
            var result = await _redisCachedService.GetAsync<Basket>(basketId);
            return result;
        }

        public async Task<Basket> UpdateBasketItemQuantityAsync(string basketId,BasketItem basketItem){
            var basket = await _redisCachedService.GetAsync<Basket>(basketId) ;
            if(basket != null){
                var targetBasketItem = basket.BasketItems.FirstOrDefault(i => i.Id == basketItem.Id);
                targetBasketItem.Quantity = basketItem.Quantity;
            }
            var update = await _redisCachedService.SetAsync<Basket>(basketId,basket);
            if(!update) return null;
            return basket;
        }

        public async Task<Basket> UpdateBasketAsync(Basket basket)
        {
            var isCreated = await _redisCachedService.SetAsync<Basket>(basket.Id,basket);
            return basket;
        }
    }
}