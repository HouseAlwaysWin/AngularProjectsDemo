using System.Runtime.CompilerServices;
using System.Linq;
using System;
using System.Threading.Tasks;
using EcommerceApi.Core.Data.Repositories.Interfaces;
using EcommerceApi.Core.Models.Entities;
using Newtonsoft.Json;
using StackExchange.Redis;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Core.Data.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        private readonly IMapper _mapper;

        public BasketRepository(
            IConnectionMultiplexer redis,
            IMapper mapper)
        {
            this._mapper = mapper;
            this._database = redis.GetDatabase();
        }

        public async Task<bool> RemoveBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<Basket> RemoveBasketItemAsync(string basketId,BasketItem basketItem){
            RedisValue data = await _database.StringGetAsync(basketId);
            Basket basket = data.IsNullOrEmpty ? null: JsonConvert.DeserializeObject<Basket>(data);
            var itemToRemove = basket.BasketItems.FirstOrDefault(b => b.Id == basketItem.Id);
            basket.BasketItems.Remove(itemToRemove);

            var basketUpdated = JsonConvert.SerializeObject(basket);
            var isUpdated =  await _database.StringSetAsync(
                basket.Id,JsonConvert.SerializeObject(basket),
                TimeSpan.FromDays(30)
            );
            return basket;
        }

        public async Task<Basket> GetBasketAsync(string basketId)
        {
            var data = await  _database.StringGetAsync(basketId);
            var result = data.IsNullOrEmpty ? null: JsonConvert.DeserializeObject<Basket>(data);
            return result;
        }

        public async Task<Basket> UpdateBasketItemQuantityAsync(string basketId,BasketItem basketItem){
            RedisValue data = await _database.StringGetAsync(basketId);
            Basket basket = data.IsNullOrEmpty ? null: JsonConvert.DeserializeObject<Basket>(data);
            BasketItem basketItemTarget = basket.BasketItems.FirstOrDefault(item => item.Id == basketItem.Id);
            basketItemTarget.LangCode = basketItem.LangCode;        
            basketItemTarget.Quantity= basketItem.Quantity;        

            var basketUpdated = JsonConvert.SerializeObject(basket);
            var isUpdated =  await _database.StringSetAsync(
                basket.Id,JsonConvert.SerializeObject(basket),
                TimeSpan.FromDays(30)
            );
            if(!isUpdated)return null;
            return basket;
        }

        public async Task<Basket> UpdateBasketAsync(Basket basket)
        {
            var isCreated = await _database.StringSetAsync(
                basket.Id,JsonConvert.SerializeObject(basket),
                TimeSpan.FromDays(30));
            if(!isCreated) return null;

            return await GetBasketAsync(basket.Id);
            

        }
    }
}