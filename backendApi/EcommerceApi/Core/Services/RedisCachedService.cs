using System.Text;
using System;
using System.Threading.Tasks;
using EcommerceApi.Core.Services.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace EcommerceApi.Core.Services
{
    public class RedisCachedService : IRedisCachedService
    {

        private readonly IDatabase _db;
        
        private readonly IConnectionMultiplexer _redis;

        public RedisCachedService(
            IConnectionMultiplexer redis)
        {
            this._redis = redis;
            this._db = this._redis.GetDatabase();
        }


        public async Task<T> GetAsync<T>(string key)
        {
            RedisValue data = await _db.StringGetAsync(key);
            if (!data.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
            return default(T);
        }

        public async Task<bool> SetAsync<T>(string key, T data, TimeSpan? time)
        {
            if (!time.HasValue)
            {
                time = TimeSpan.FromDays(30);
            }

            var result = await _db.StringSetAsync(
                  key, JsonConvert.SerializeObject(data),
                  time
            );

            return result;

        }

        public async Task<T> GetAndSetAsync<T>(string key, T data, TimeSpan? time=null)
        {
            var cachedData = await GetAsync<T>(key);
            if (cachedData != null)
            {
                return cachedData;
            }
            var result= await SetAsync<T>(key, data, time);
            if(result){
                return await GetAsync<T>(key);
            }
            return default(T);
        }

        public async Task<T> GetAndSetAsync<T>(string key, Func<Task<T>> acquire, TimeSpan? time=null)
        {
            var cachedData = await GetAsync<T>(key);
            if (cachedData != null)
            {
                return cachedData;
            }
            var data = await acquire();

            if(data !=null){
                await SetAsync<T>(key, data, time);
            }

            return data;
        }

        public string CreateKey<T>(params object[] param){
            string typeName = typeof(T).Name;
            StringBuilder builder  = new StringBuilder($"defaultKey_{typeName}");
            foreach (var p in param)
            {
               builder.Append(p);
               builder.Append("_");
            }
            return builder.ToString();
        }

    }
}