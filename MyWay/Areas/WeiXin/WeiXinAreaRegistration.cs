using System.Web.Mvc;

namespace MyWay.Areas.WeiXin
{
    public class WeiXinAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "WeiXin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                "WeiXin_default",
                "w/{controller}/{action}/{id}",
                new {controller = "WeiXin", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
