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
        private readonly IEntityRepository<Language> _languageEntityRepo;
        private readonly IRedisCachedService _redisCachedService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LanguageService(
            IRedisCachedService redisCachedService,
            IHttpContextAccessor httpContextAccessor,
            IEntityRepository<Language> languageEntityRepo
            )
        {
            this._languageEntityRepo = languageEntityRepo;
            this._redisCachedService = redisCachedService;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<Language> GetCurrentLanguageAsync()
        {
            var methodName = nameof(GetCurrentLanguageAsync);
            var key = _redisCachedService.CreateKey<Language>(methodName);
            var currentLang = _httpContextAccessor.HttpContext.Request.Headers["Accept-Language"].FirstOrDefault();

            var langList = await _redisCachedService.GetAndSetAsync<List<Language>>(key, async () =>
            {
                var source =await _languageEntityRepo.GetAllAsync();
                return source.ToList();;
            });

            var langResult = langList.FirstOrDefault(l => l.LangCulture == currentLang);
            return langResult;
        }
    }
}