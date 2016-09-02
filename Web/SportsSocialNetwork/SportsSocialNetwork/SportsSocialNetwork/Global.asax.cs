using AutoMapper;
using SkyWeb.DatVM.Mvc.Autofac;
using SportsSocialNetwork.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Teek.Models.AutofacModules;

namespace SportsSocialNetwork
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        protected void Application_Start()
        {
            //custom route
            RouteTable.Routes.MapMvcAttributeRoutes();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutofacInitializer.Initialize(
                System.Reflection.Assembly.GetExecutingAssembly(),
                typeof(SSNEntities),

                // AutoMapper config
                new MapperConfiguration(this.MapperConfig),

                // Autofac Modules
                new FileUploaderModule());

        }


        private void MapperConfig(IMapperConfiguration config)
        {
            config.CreateMissingTypeMaps = true;

        }


    }
}
