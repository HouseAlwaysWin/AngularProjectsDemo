using System;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace EcommerceApi.Helpers.Localization
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {

        private readonly string _resourcesRelativePath;
        public JsonStringLocalizerFactory(
             IOptions<LocalizationOptions> localizationOptions)
        {
            if (localizationOptions == null)
            {
                throw new ArgumentNullException(nameof(localizationOptions));
            }
            _resourcesRelativePath = localizationOptions.Value.ResourcesPath ?? String.Empty;
        }
        public IStringLocalizer Create(string baseName, string location)
        {
             return new JsonStringLocalizer(_resourcesRelativePath); 
        }

        public IStringLocalizer Create(Type resourceSource)
        {
             return new JsonStringLocalizer(_resourcesRelativePath); 
        }
    }
}