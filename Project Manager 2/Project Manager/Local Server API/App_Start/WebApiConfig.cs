using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Local_Server_API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // added to solve the reference loop problem
            var JsonConf = GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;

            JsonConf.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            JsonConf.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Added route for multiple actions in one controller support
            config.Routes.MapHttpRoute(
                "DefaultApiWithAction",
                "Api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional });
        }
    }
}
