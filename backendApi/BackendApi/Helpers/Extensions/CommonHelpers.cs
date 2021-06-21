using Newtonsoft.Json;

namespace BackendApi.Helpers.Extensions
{
    public static class CommonHelpers
    {
        public static T CloneObject<T>(T obj) {
            var serializeObj = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(serializeObj);
        }
    }
}