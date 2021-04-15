using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using EcommerceApi.Core.Data.Repositories.Interfaces;
using EcommerceApi.Core.Models.Entities;
using EcommerceApi.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EcommerceApi.Core.Services
{
  

    public class LocalizedService : ILocalizedService
    {
        private readonly IEntityRepository<Localized> _localizedEntityRepo;
        private readonly ILanguageService _languageService;
        private readonly IRedisCachedService _redisCachedService;

        public LocalizedService(
            IRedisCachedService redisCachedService,
            IEntityRepository<Localized> localizedEntityRepo,
            ILanguageService languageService
        )
        {
            this._redisCachedService = redisCachedService;
            this._localizedEntityRepo = localizedEntityRepo;
            this._languageService = languageService;
        }

        public async Task<string> GetLocalizedAsync<TEntity, TProp>(TEntity entity,
            Expression<Func<TEntity, TProp>> keySelector, int? languageId =null)
        where TEntity : BaseEntity
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
                var source = await _localizedEntityRepo.GetAllAsync();
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