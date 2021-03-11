using System.Collections.Generic;

namespace EcommerceApi.Helpers.Localization
{
    internal class JsonLocalizationFormat
    {
        public string Key { get; set; }
        public Dictionary<string,string> Value { get; set; }
    }
}