using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Models.Entities;
using BackendApi.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BackendApi.Core.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly IStoreRepository<Language> _languageEntityRepo;
        private readonly ICachedService _redisCachedService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LanguageService(
            ICachedService redisCachedService,
            IHttpContextAccessor httpContextAccessor,
            IStoreRepository<Language> languageEntityRepo
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