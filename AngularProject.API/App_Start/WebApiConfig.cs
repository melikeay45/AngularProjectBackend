using AngularProject.API.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AngularProject.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            //Projede dışardan gelen isteklere izin verir.
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            config.MessageHandlers.Add(new TokenValidationHandler());


        }
    }
}
