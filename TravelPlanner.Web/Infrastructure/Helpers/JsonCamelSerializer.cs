using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TravelPlanner.Web.Infrastructure.Helpers
{
    public static class JsonCamelSerializer
    {
        public static string Serialize(this object obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}
