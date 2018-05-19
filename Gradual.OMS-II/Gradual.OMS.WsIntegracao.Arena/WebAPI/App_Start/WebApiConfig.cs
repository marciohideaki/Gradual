using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Gradual.OMS.WsIntegracao.Arena.Controllers;


namespace Gradual.OMS.WsIntegracao.Arena
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MessageHandlers.Add(new MethodOverrideHandler());

            //config.EnableCors(new EnableCorsAttribute("*", "*", "GET,PUT,POST,DELETE,OPTIONS"));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "gradualapi/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
    }
}
