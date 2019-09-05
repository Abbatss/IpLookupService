using IPLookup.API.Host.Controllers;
using IPLookup.API.Host.Utilities;
using IPLookup.API.InMemoryDataBase;
using IPLookup.API.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;

namespace IPLookup.API.Host.App_Start
{
    public static class DIConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var services = new ServiceCollection();
            services.AddControllersAsServices(GetControllers());
            var dbPath = HostingEnvironment.MapPath(@"~/bin/DataBase/geobase.dat");
            var client = new GeoDataBaseClient(dbPath);
            client.Init();
            services.AddSingleton<IGeoDataBaseQuery>(new GeoDataBaseQuery(client));
            var resolver = new CustomDependencyResolver(services.BuildServiceProvider());
            config.DependencyResolver = resolver;

        }
        private static IEnumerable<Type> GetControllers()
        {
            return typeof(WebApiApplication).Assembly.GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
            .Where(t => typeof(IController).IsAssignableFrom(t)
            || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase));
        }
    }
}