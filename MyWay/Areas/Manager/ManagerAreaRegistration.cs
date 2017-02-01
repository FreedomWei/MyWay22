using System.Web.Mvc;

namespace MyWay.Areas.Manager
{
    public class ManagerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Manager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                name:"contentPage",
                url: "M/{controller}/{action}/{id}",
                defaults: new {controller = "Home",action = "Index",id=UrlParameter.Optional},
                namespaces: new string[] { "MyWay.Areas.Manager.Controllers" }
                );


            context.MapRoute(
                "Manager_default",
                "M/{controller}/{action}/{id}",
                new { action = "Index",controller = "Home", id = UrlParameter.Optional },
                namespaces:new string[] { "MyWay.Areas.Manager.Controllers" }
            );
        }
    }
}
