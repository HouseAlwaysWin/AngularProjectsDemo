using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Services.Interfaces
{
    public interface ILocalizedService
    {
        Task<string> GetLocalizedAsync<TEntity, TProp>(TEntity entity, Expression<Func<TEntity, TProp>> keySelector, int? languageId =null) 
                where TEntity : BaseEntity;
        Task<Localized> GetLocalizedObjectAsync(int languageId, int tableId, string localeTable, string localeKey);
    }
}