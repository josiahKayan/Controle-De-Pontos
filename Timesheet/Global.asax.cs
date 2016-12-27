using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using System.Web.Routing;
using Apassos.Models;

using Quartz;
using Quartz.Impl;
using Apassos.TeamWork.Jobs;
using Ninject;
using Ninject.Web.Mvc;
using Apassos.IoC;
using System.Collections.Generic;
using Apassos.DataAccess;
using Apassos.TeamWork.Parsers;
using Apassos.TeamWork.Services;

namespace Apassos
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly string DATASOURCE = @"Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True";

        public static ISchedulerFactory sfApplication = new StdSchedulerFactory();
        public static IScheduler scApplication;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new OutputCacheAttribute
            {
                VaryByParam = "*",
                Duration = 0,
                NoStore = true,
            });

            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.aspx/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Login", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            //descomentar: desabilita a criacao da base automatica
            Database.SetInitializer<TimesheetContext>(null);

            AreaRegistration.RegisterAllAreas();

            // Use LocalDB for Entity Framework by default
            Database.DefaultConnectionFactory = new SqlConnectionFactory(DATASOURCE);

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            StartTeamWorkRetrieverScheduler();

            InitDependencies();

            
        }

        private void InitDependencies()
        {
            DependencyResolver.SetResolver(
                      new NinjectDependencyResolver(new StandardKernel(new TimesheetNinjectModule())));
        }

        private void StartTeamWorkRetrieverScheduler()
        {
            TeamWorkScheduler job = new TeamWorkScheduler();
            job.Start();
        }
    }
}