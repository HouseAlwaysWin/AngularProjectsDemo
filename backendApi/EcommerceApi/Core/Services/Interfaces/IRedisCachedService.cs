using System;
using System.Threading.Tasks;

namespace EcommerceApi.Core.Services.Interfaces
{
    public interface IRedisCachedService
    {
        Task<T> GetAndSetAsync<T>(string key, T data, TimeSpan? time = null);
        Task<T> GetAndSetAsync<T>(string key, Func<Task<T>> acquire, TimeSpan? time=null);
        Task<T> GetAsync<T>(string key);
        Task<bool> SetAsync<T>(string key, T data, TimeSpan? time =null);
        string CreateKey<T>(params object[] param);
    }
}