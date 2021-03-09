using System.Collections.Specialized;
using System.Text;
using System;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.Extensions.Localization;

namespace EcommerceApi.Helpers.Localization
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly string _resourcesRelativePath;
        public JsonStringLocalizer(string resourcesRelativePath)
        {
            _resourcesRelativePath = resourcesRelativePath;
        }

        public LocalizedString this[string name] => throw new System.NotImplementedException();

        public LocalizedString this[string name, params object[] arguments] => throw new System.NotImplementedException();

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new System.NotImplementedException();
        }
    }
}