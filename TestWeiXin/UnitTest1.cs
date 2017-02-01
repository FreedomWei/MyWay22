using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using MyWay.Areas.WeiXin.Controllers;
using MyWay.Areas.WeiXin.Test;
using com.yajingling.Tests.tool;
using System.Net;
using System.Text;
using System.Web.Mvc;
using MyWay.Controllers;
using Moq;

namespace TestWeiXin
{
    [TestClass]
    public class UnitTest1 : BaseTest
    {

        [TestMethod]
        public void IndexActionTest()
        {
            var homeController = new XXController();
            homeController.ControllerContext = MvcContextMockFactory.CreateControllerContext(homeController, "~/Home/Index", "get", "DefaultRoute", "{controller}/{action}", null);
            ViewResult result = homeController.Index();
            Assert.AreEqual("Index", result.ViewName);
            Assert.AreEqual("Home", result.ViewData["controller"]);
            Assert.AreEqual("Index", result.ViewData["action"]);
        }

        //[TestMethod]
        //public void IndexActionTest1()
        //{
        //    var homeController = new WeiXinController();
        //    homeController.ControllerContext = MvcContextMockFactory.CreateControllerContext(homeController, "~/w/weixin/Index", "get", "WeiXin_default", "{controller}/{action}", null);
        //    System.Web.HttpContext b = new System.Web.HttpContext(new System.Web.HttpRequest("f","w","s"),new System.Web.HttpResponse(TextWriter.Null));
        //    homeController.Index(b);
        //}

        
    }
}
