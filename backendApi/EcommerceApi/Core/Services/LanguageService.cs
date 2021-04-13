using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EcommerceApi.Core.Data.Repositories.Interfaces;
using EcommerceApi.Core.Models.Entities;
using EcommerceApi.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EcommerceApi.Core.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly IGenericRepository<Language> _languageGenericService;
        private readonly IRedisCachedService _redisCachedService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LanguageService(
            IRedisCachedService redisCachedService,
            IHttpContextAccessor httpContextAccessor,
            IGenericRepository<Language> languageGenericService
            )
        {
            this._languageGenericService = languageGenericService;
            this._redisCachedService = redisCachedService;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<Language> GetCurrentLanguageAsync()
        {
            var methodName = nameof(GetCurrentLanguageAsync);
            var key = _redisCachedService.CreateKey<Language>(methodName);
            var currentLang = _httpContextAccessor.HttpContext.Request.Headers["Accept-Language"].FirstOrDefault();

            var langList = await _redisCachedService.GetAndSetAsync<IReadOnlyList<Language>>(key, async () =>
            {
                var source = await _languageGenericService.ListAllAsync();
                return source;
            });

            var langResult = langList.FirstOrDefault(l => l.LangCulture == currentLang);
            return langResult;
        }
    }
}