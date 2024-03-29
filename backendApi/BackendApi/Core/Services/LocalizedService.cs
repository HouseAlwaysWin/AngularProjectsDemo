using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Models.Entities;
using BackendApi.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BackendApi.Core.Services
{
  

    public class LocalizedService : ILocalizedService
    {
        private readonly IStoreRepository _localizedEntityRepo;
        private readonly ILanguageService _languageService;
        private readonly ICachedService _redisCachedService;

        public LocalizedService(
            ICachedService redisCachedService,
            IStoreRepository localizedEntityRepo,
            ILanguageService languageService
        )
        {
            this._redisCachedService = redisCachedService;
            this._localizedEntityRepo = localizedEntityRepo;
            this._languageService = languageService;
        }

        public async Task<string> GetLocalizedAsync<TEntity, TProp>(TEntity entity,
            Expression<Func<TEntity, TProp>> keySelector, int? languageId =null)
        where TEntity : class,IBaseEntity
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (!(keySelector.Body is MemberExpression member))
            {
                throw new ArgumentException($"Expression '{keySelector}' refers to a method, not a property.");
            }

            if (!(member.Member is PropertyInfo propInfo))
            {
                throw new ArgumentException($"Expression '{keySelector}' refers to a field, not a property.");
            }

            if(!languageId.HasValue){
                var lang = await _languageService.GetCurrentLanguageAsync();
                languageId = (lang==null)? 1 : lang.Id;
            }

            var entityType = entity.GetType().Name;
            var propertyKey = propInfo.Name;

            if (languageId > 0)
            {
                Localized locale = await GetLocalizedObjectAsync(languageId.Value, entity.Id, entityType, propertyKey);
                if (locale != null)
                {
                    return locale.PropertyValue;
                }
            }

            var defaultValue = propInfo.GetValue(entity).ToString();
            return defaultValue;
        }

        public async Task<Localized> GetLocalizedObjectAsync(int languageId, int tableId, string entityType, string propertyKey)
        {
            var key = _redisCachedService.CreateKey<Localized>(languageId, tableId, entityType, propertyKey);
            return await _redisCachedService.GetAndSetAsync<Localized>(key, async () =>
            {
                var source = await _localizedEntityRepo.GetAllAsync<Localized>();
                if (source != null)
                {
                    var query = source.Where(l =>
                     (l.LanguageId == languageId) &&
                     (l.TableId == tableId) &&
                     (l.EntityType == entityType) &&
                     (l.PropertyKey == propertyKey)).FirstOrDefault();
                    return query;
                }
                return null;
            });
        }
    }
}