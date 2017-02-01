using Mldel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWay.Attribute
{
    public class UserFilterAttribute:ActionFilterAttribute
    {
        private bool loginCheck;
        public UserFilterAttribute(bool login = false)
        {
            this.loginCheck = login;
        }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
       {
            if (loginCheck) return;
            HttpContextBase context = filterContext.HttpContext;
            HttpRequestBase request = context.Request;
            HttpResponseBase response = context.Response;
            HttpSessionStateBase Session = context.Session;
            if (Session == null || Session["admin"] == null)
            {
                response.Redirect("/m/home/Login");
                filterContext.Result = new EmptyResult();
                return;
            }
        }

    }
}