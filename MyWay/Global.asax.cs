using MyWay.Areas.WeiXin.WeiXinModel;
using MyWay.ErrorLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MyWay
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static string DirSiteFile;
        public static string DirParent;
        protected void Application_Start()
        {
            Application["OnLineUserCount"] = 0;
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DirParent = Server.MapPath("~/");
            DirSiteFile = DirParent + "ErrorDetailLog\\";
            Log.LogWritePath =string.Format(DirSiteFile+"Error\\{{0:ddHHmm}}.txt");
            WangZhanPath._DirSiteFile = DirSiteFile;
            //加入测试公众号
            AccountInfoCollection.SetAccountInfo(new AccountInfo("测试公众号", "wxbf1387e25455a9e3", "a680e1ecaa818a7a25f9516ffe457c48", "cjwFreedom", "45oa1QcJZ5tcmDEA6N1BzJLmHasol8tfPKEAlnj5bVv", "测试公众号"));
            //个人订阅号
            //AccountInfoCollection.SetAccountInfo(new AccountInfo("第一维", "wx1d85b4b8b1daa55c", "7ae69a5f17c19340dde77bd017f4ca2e ", "cjwFreedom", "45oa1QcJZ5tcmDEA6N1BzJLmHasol8tfPKEAlnj5bVv", "第一维"));
        }


        /// <summary>
        /// 自定义404页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex is HttpException && ((HttpException)ex).GetHttpCode() == 404)
            {
                //如何指向绝对路径的URL?
                Response.Redirect("/404.html");
            }
            if (ex is HttpException && ((HttpException)ex).GetHttpCode() == 500)
            {
                //如何指向绝对路径的URL?
                Response.Redirect("/404.html");
            }
        }



        protected void Session_Start(object sender, EventArgs e)
        {
            Application.Lock();
            Application["OnLineUserCount"] = Convert.ToInt32(Application["OnLineUserCount"]) + 1;
            int num = (int)System.Web.HttpContext.Current.Application["OnLineUserCount"];
            Application.UnLock();
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application["OnLineUserCount"] = Convert.ToInt32(Application["OnLineUserCount"]) - 1;
            Application.UnLock();
        }
        protected void Application_End()
        {
            Application.Lock();
            Application["OnLineUserCount"] = Convert.ToInt32(Application["OnLineUserCount"]) - 1;
            Application.UnLock();
        }

    }
}