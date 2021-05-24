using System;
using BackendApi.Core.Services.Interfaces;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace BackendApi.Helpers.Localization
{
    public class JsonStringLocalizerFactory:IStringLocalizerFactory
    {

        private readonly IOptions<LocalizationOptions> _localizationOptions;
        private readonly ICachedService  _cachedService;
        public JsonStringLocalizerFactory(
             IOptions<LocalizationOptions> localizationOptions,
             ICachedService cachedService)
        {
            this._localizationOptions = localizationOptions;
            this._cachedService=cachedService;
        }
        public IStringLocalizer Create(Type resourceSource)
        {
            return new JsonStringLocalizer(_localizationOptions,_cachedService);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new JsonStringLocalizer(_localizationOptions,_cachedService);
        }

        
    }
}