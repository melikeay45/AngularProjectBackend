using AngularProject.DTO.MappingProfiles;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AngularProject.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //Tabloları profilleriyle eşleştirir. Profiles klasöründe tanıttığım her profili burda belirtiyorum
            AutoMapper.Mapper.Initialize(cfg =>
            {
                //mapping tables

                cfg.AddProfile<OrderProfile>();
                cfg.AddProfile<ProductProfile>();
                cfg.AddProfile<CategoryProfile>();
                cfg.AddProfile<ShoppingCartProfile>();
                cfg.AddProfile<UserProfile>();
                cfg.AddProfile<LogProfile>();

            });


        }
     
    }
}
