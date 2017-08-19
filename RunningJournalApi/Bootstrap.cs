using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace RunningJournalApi
{
    public class Bootstrap
    {
        public void Configure(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "API Default",
                routeTemplate: "{controller}/{id}",
                defaults: new
                {
                    controller = "Journal",
                    id = RouteParameter.Optional
                });
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
        }
    }
}