using Moq;
using MyWay.Areas.WeiXin.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyWay.Areas.WeiXin
{
    public static class XXX
    {
        
        public static HttpContextBase FakeHttpContext(Stream inputStream = null, string userAgent = null)
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();
            // var requestInputStream = new Mock<Stream>();

            //* 如果出现错误：System.ArgumentException: Unable to obtain public key for StrongNameKeyPair
            //* 是因为无法对MachineKeys目录进行写和删除操作，
            //* 需要给这个文件夹重设权限（Administrators has full control, everyone has had read/write access, but not delete access.）：
            //* C:\Documents and Settings\All Users\Application Data\Microsoft\Crypto\RSA
            //* 参考：http://ayende.com/blog/1441/unable-to-obtain-public-key-for-strongnamekeypair
            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);

            request.Setup(r => r.InputStream).Returns(inputStream);
            request.Setup(r => r.UserAgent).Returns(userAgent);
            request.Setup(r => r.Url).Returns(new Uri("http://weixin.senparc.com"));

            return context.Object;
        }

        public static void SetFakeControllerContext(this WeiXinController controller, Stream inputStream = null, string userAgent = null)
        {
            var httpContext = FakeHttpContext(inputStream, userAgent);
            ControllerContext context = new ControllerContext(new RequestContext(httpContext, new RouteData()), controller);
            controller.ControllerContext = context;
        }

    }
}