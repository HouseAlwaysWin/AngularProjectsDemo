using System;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace EcommerceApi.Helpers.Localization
{
    public class JsonStringLocalizerFactory:IStringLocalizerFactory
    {

        private readonly IOptions<LocalizationOptions> _localizationOptions;
        public JsonStringLocalizerFactory(
             IOptions<LocalizationOptions> localizationOptions)
        {
            this._localizationOptions = localizationOptions;
        }
        public IStringLocalizer Create(Type resourceSource)
        {
            return new JsonStringLocalizer(_localizationOptions);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new JsonStringLocalizer(_localizationOptions);
        }

        
    }
}