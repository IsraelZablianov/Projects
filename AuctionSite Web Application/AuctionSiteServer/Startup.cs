using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

[assembly: OwinStartup(typeof(AuctionSiteServer.Startup))]

namespace AuctionSiteServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            configuration.MapHttpAttributeRoutes();
            configuration.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DateParseHandling = DateParseHandling.None
            };

            app.UseWebApi(configuration);
            //I.Z maping signlR
            app.MapSignalR();
        }
    }
}
