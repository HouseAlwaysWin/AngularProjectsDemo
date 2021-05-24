using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BackendApi.Core.Models.Entities;

namespace BackendApi.Core.Services.Interfaces
{
    public interface ILocalizedService
    {
        Task<string> GetLocalizedAsync<TEntity, TProp>(TEntity entity, Expression<Func<TEntity, TProp>> keySelector, int? languageId =null) 
                where TEntity : class,IBaseEntity;
        Task<Localized> GetLocalizedObjectAsync(int languageId, int tableId, string localeTable, string localeKey);
    }
}