//using OnlineBikeStore.App_Start;
using OnlineBikeStore.AutoMapperConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OnlineBikeStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapper.Mapper.Initialize(config:cfg=>cfg.AddProfile<AutomapperProfile>());
            //RotativaConfig.Setup();

        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {                                
                if (User.IsInRole("admin"))
                {
                    Response.Redirect("~/Dashboard/Dashboard");
                }
                else if (User.IsInRole("customer"))
                {
                    Response.Redirect("~/Home/Home");
                }
                else
                {
                    Response.Redirect("~/Home/Index");
                }
            }
        }

    }
}
