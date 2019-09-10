using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(   //url/
                 null,
                "",
                 new { controller = "Book", action = "List",
                     specilization= (string)null,pageno = 1 }
            );

            routes.MapRoute( //url/ BookListPage2
                 null,
                "BookListPage{pageno}",
                 new
                 {
                     controller = "Book",
                     action = "List",
                     specilization = (string)null,
                     
                 }
            );

            routes.MapRoute(   //url / is
                 null,
                "{specilization}",
                 new
                 {
                     controller = "Book",
                     action = "List",
                     pageno = 1
                 }
            );

            routes.MapRoute(   //url/is/page2
                 null,
                "{specilization}/Page{pageno}",
                 new
                 {
                     controller = "Book",
                     action = "List",
                     
                 },
                 //elly ben el "" hayt7at zy ma hoa
                 new { pageno = @"\d+"}
            );



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {  id = UrlParameter.Optional }
            );
        }
    }
}
