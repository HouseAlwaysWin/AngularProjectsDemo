using System.Collections.Specialized;
using System.Text;
using System;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using BackendApi.Core.Services.Interfaces;
using BackendApi.Core.Services;

namespace BackendApi.Helpers.Localization
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private Dictionary<string,string> localization;
        private string _resourcesRelativePath;
        private readonly IOptions<LocalizationOptions> _localizationOptions;
        private readonly ICachedService  _cachedService;
        public JsonStringLocalizer(
            IOptions<LocalizationOptions> localizationOptions,
            ICachedService cachedService
        )
        {
            this._cachedService = cachedService;
            this._localizationOptions = localizationOptions;

            var lang = CultureInfo.CurrentCulture.Name;
            _resourcesRelativePath = localizationOptions.Value.ResourcesPath ?? String.Empty;
            CheckResourcePath(_resourcesRelativePath,lang);
        }

        private void CheckResourcePath(string path,string langName){
          localization =  this._cachedService.GetAndSet<Dictionary<string,string>>(langName,()=>{
                    path = string.IsNullOrEmpty(path) ? Directory.GetCurrentDirectory()+"\\Resources":path;
                    var resourcePath = path + $"\\{langName}.json";

                    if(!File.Exists(resourcePath)){
                        Directory.CreateDirectory(path);
                        using (System.IO.FileStream fs = System.IO.File.Create(resourcePath)){
                            string data = "{}";
                            byte[] dataInfo = new UTF8Encoding(true).GetBytes(data);
                            fs.Write(dataInfo,0,dataInfo.Length);
                        }
                    }

                     return JsonConvert.DeserializeObject<Dictionary<string,string>>(
                                    File.ReadAllText(resourcePath));

                    });
            }

        public LocalizedString this[string name]  {
            get{
                var value = GetString(name);
                return new LocalizedString(
                    name, value ?? name, resourceNotFound: value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments] {
            get{
               var value = GetString(name); 
               value = string.Format(value,arguments);
               return new LocalizedString(
                    name, value ?? name, resourceNotFound: value == null);

            }

        }
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            List<LocalizedString> allStrings = new List<LocalizedString>();
            if(!includeParentCultures){
                foreach (var item in localization)
                {
                    allStrings.Add(new LocalizedString(item.Key,item.Value));
                }
                return allStrings;
            }

            string[] subdirs = Directory.GetDirectories(_resourcesRelativePath)
                            .Select(Path.GetFileName)
                            .ToArray();
            
            foreach(var fileName in subdirs){
                CheckResourcePath(_resourcesRelativePath,fileName);
                foreach (var item in localization)
                {
                    allStrings.Add(new LocalizedString(item.Key,item.Value,true,fileName));
                }
            }

            return allStrings;
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            CultureInfo.CurrentCulture = culture;
            return new JsonStringLocalizer(_localizationOptions,_cachedService);
        }

        private string GetString(string name)
        {
            if(localization.ContainsKey(name)){
                return localization[name];
            }
            return name;

        }
    }
}