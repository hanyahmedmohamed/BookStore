using BookStore.Domain.Entities;
using BookStore.WebUI.Infrastructure.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookStore.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //hat2om m3 bdayt el project
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //HARBOT EL state
            ModelBinders.Binders.Add(typeof(Cart),new CartModelBinder());
        }
    }
}
