using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using WfAssist.Logic;

namespace WfAssist
{
    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{action}/{language}/{pattern}/{availableLetters}",
                new
                {
                    controller = "Home",
                    action = "Index",
                    language = UrlParameter.Optional,
                    pattern = UrlParameter.Optional,
                    availableLetters = UrlParameter.Optional
                });
        }

        private void RegisterAutoFac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder
                .RegisterType<WordProvider>()
                .As<IWordProvider>()
                .SingleInstance();
                
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        protected void Application_Start()
        {
            RegisterAutoFac();

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}