using System;
using System.Threading.Tasks;
using EcommerceApi.Core.Data.Repositories.Interfaces;
using EcommerceApi.Core.Models.Entities;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace EcommerceApi.Core.Data.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            this._database = redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<Basket> GetBasketAsync(string basketId)
        {
            var data = await  _database.StringGetAsync(basketId);
            var result = data.IsNullOrEmpty ? null: JsonConvert.DeserializeObject<Basket>(data);
            return result;
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