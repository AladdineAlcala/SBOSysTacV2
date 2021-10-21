using System.Web.Mvc;
using System.Web.Routing;

namespace SBOSysTacV2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.aspx/{*pathInfo}");
            routes.IgnoreRoute("Reports/ReportViewers/{resource}.aspx/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SBOSysTacV2.Controllers" }
            );

            routes.MapRoute
            (
                name: "BookMenustoPackage",
                url: "{controller}/{action}/{transacId}/{menuId}",
                defaults: new { controller = "Bookings", action = "AddMenusToPackage", transacId = UrlParameter.Optional, mennuId = UrlParameter.Optional }
            );

            routes.MapRoute
            (
                name: "PrintContract",
                url: "{controller}/{action}/{transId}/{selprintopt}",
                defaults: new { controller = "Bookings", action = "PrintContract", transId = UrlParameter.Optional, selprintopt = UrlParameter.Optional }
            );
        }
    }
}