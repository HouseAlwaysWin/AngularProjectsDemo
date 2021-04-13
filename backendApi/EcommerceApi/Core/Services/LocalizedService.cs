using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using EcommerceApi.Core.Data.Repositories.Interfaces;
using EcommerceApi.Core.Models.Entities;
using EcommerceApi.Core.Services.Interfaces;

namespace EcommerceApi.Core.Services
{
  

    public class LocalizedService : ILocalizedService
    {
        private readonly IGenericRepository<Localized> _localizedGenericRepo;
        private readonly ILanguageService _languageService;
        private readonly IRedisCachedService _redisCachedService;

        public LocalizedService(
            IRedisCachedService redisCachedService,
            IGenericRepository<Localized> localizedGenericRepo,
            ILanguageService languageService
        )
        {
            this._redisCachedService = redisCachedService;
            this._localizedGenericRepo = localizedGenericRepo;
            this._languageService = languageService;
        }

        public async Task<string> GetLocalizedAsync<TEntity, TProp>(TEntity entity,
            Expression<Func<TEntity, TProp>> keySelector, int? languageId =null)
        where TEntity : BaseEntity, ILocalizedEntity
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
                languageId = lang.Id;
            }

            var localeTable = entity.GetType().Name;
            var localeKey = propInfo.Name;

            if (languageId > 0)
            {
                Localized locale = await GetLocalizedObjectAsync(languageId.Value, entity.Id, localeTable, localeKey);
                if (locale != null)
                {
                    return locale.LocaleValue;
                }
            }

            return string.Empty;
        }

        public async Task<Localized> GetLocalizedObjectAsync(int languageId, int tableId, string localeTable, string localeKey)
        {
            var key = _redisCachedService.CreateKey<Localized>(languageId, tableId, localeTable, localeKey);
            return await _redisCachedService.GetAndSetAsync<Localized>(key, async () =>
            {
                var source = await _localizedGenericRepo.ListAllAsync();
                if (source != null)
                {
                    var query = source.Where(l =>
                     (l.LanguageId == languageId) &&
                     (l.TableId == tableId) &&
                     (l.LocaleTable == localeTable) &&
                     (l.LocaleKey == localeKey)).FirstOrDefault();
                    return query;
                }
                return null;
            });
        }
    }
}