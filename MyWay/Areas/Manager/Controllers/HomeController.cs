using BLL;
using Mldel;
using Mldel.Content;
using Mldel.Contents;
using Mldel.User;
using MyWay.Attribute;
using MyWay.ErrorLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWay.Areas.Manager.Controllers
{
    [UserFilter]
    public class HomeController : Controller
    {
        //
        // GET: /Manager/Home/

        public ActionResult Index()
        {
            return View();
        }



        public ActionResult ContentPage()
        {
            return View();
        }


        public ActionResult Edit()
        {
            return View();
        }

        /// <summary>
        /// 根据Id获取文章
        /// </summary>
        /// <returns></returns>
        public string GetContentById()
        {
            try
            {
                 int id = Convert.ToInt32(Request["contentId"]);
                 ContentBLL bll = new ContentBLL();
                 ContentModel model = bll.GetContentById(id);
                 return "success:"+ JsonConvert.SerializeObject(model).ToString();
            }
            catch (Exception)
            {
                return "failure:失败";
            }
        }




        /// <summary>
        /// 文章管理页面
        /// </summary>
        /// <returns></returns>
        //[OutputCache(CacheProfile = "Long")]
        public string GetCList()
        {
            try
            {
                //搜索内容
                string search = Request["txt_search"] ?? "";
                //页码
                int index = Convert.ToInt32(Request["index"] == "" || Request["index"] == null ? "1" : Request["index"]);
                //每页取几条数据
                int page = Convert.ToInt32(Request["pagesize"]);
                //总数
                int total = Convert.ToInt32(Request["Total"]);
                //类别id
                int typeId = Convert.ToInt32(Request["typeId"]);
                ContentBLL bll = new ContentBLL();
                List<TypeModel> tlist = bll.GetTyleList();
                TableModel<ContentModel> info = bll.GetContentList(search, index, page, total,typeId);
                var res = JsonConvert.SerializeObject(info);
                return "success:" + res.ToString();
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "failure:失败";
            }
        }


        /// <summary>
        /// 标签管理页
        /// </summary>
        /// <returns></returns>
        public ActionResult LabelPage()
        {
            return View();
        }

        /// <summary>
        /// 类别管理页
        /// </summary>
        /// <returns></returns>
        public ActionResult TypePage()
        {
            return View();
        }


        public string GetTypes()
        {
            try
            {
                //搜索内容
                string search = Request["txt_search"] ?? "";
                //页码
                int index = Convert.ToInt32(Request["index"] == "" || Request["index"] == null ? "1" : Request["index"]);
                //每页取几条数据
                int page = Convert.ToInt32(Request["pagesize"]);
                //总数
                int total = Convert.ToInt32(Request["Total"]);
                ContentBLL bll = new ContentBLL();
                TableModel<TypeModel> info = bll.GetTypes(search, index, page, total);
                var res = JsonConvert.SerializeObject(info);
                return "success:" + res.ToString();
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "failure:失败";
            }
        }





        /// <summary>
        /// 获取标签
        /// </summary>
        /// <returns></returns>
        public string GetLabels()
        {
            try
            {
                //搜索内容
                string search = Request["txt_search"] ?? "";
                //页码
                int index = Convert.ToInt32(Request["index"] == "" || Request["index"] == null ? "1" : Request["index"]);
                //每页取几条数据
                int page = Convert.ToInt32(Request["pagesize"]);
                //总数
                int total = Convert.ToInt32(Request["Total"]);
                UserBLL bll = new UserBLL();
                TableModel<LabelModel> info = bll.GetLabels(search, index, page, total);
                var res = JsonConvert.SerializeObject(info);
                return "success:" + res.ToString();
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "failure:失败";
            }
        }

        /// <summary>
        /// 编辑类别
        /// </summary>
        /// <returns></returns>
        public string UpdateType()
        {
            try
            {
                string lid = Request["id"];
                string name = Request["name"];
                int id = 0;
                int.TryParse(lid, out id);
                ContentBLL bll = new ContentBLL();
                if (id == 0)
                {

                    int res = bll.AddType(name);
                    if (res > 0)
                    {
                        return "success";
                    }
                }
                else
                {
                    int res = bll.UpdateType(id, name);
                    if (res > 0)
                    {
                        return "success";
                    }
                }

                return "failure:失败";
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "failure:失败";
            }
        }

        /// <summary>
        /// 编辑标签
        /// </summary>
        /// <returns></returns>
        public string UpdateLabel()
        {
            try
            {
                string lid =Request["id"];
                string name = Request["name"];
                int id = 0;
                int.TryParse(lid,out id);
                ContentBLL bll = new ContentBLL();
                if (id == 0)
                {

                    int res = bll.AddLabel(name);
                    if (res > 0)
                    {
                        return "success";
                    }
                }
                else
                {
                    int res = bll.UpdateLabel(id, name);
                    if (res > 0)
                    {
                        return "success";
                    }
                }
                 
                return "failure:失败";
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "failure:失败";
            }
        }

        /// <summary>
        /// 检查标签是否存在
        /// </summary>
        /// <returns></returns>
        public string CheckLabel()
        {
            try
            {
                string name = Request["name"];
                ContentBLL bll = new ContentBLL();
                bool check = bll.ChekcLabel(name);
                if (check)
                {
                    return "success:ok";
                }
                else
                {
                    return "success:nook";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }



        /// <summary>
        /// 检查类别是否存在
        /// </summary>
        /// <returns></returns>
        public string CheckType()
        {
            try
            {
                string name = Request["name"];
                ContentBLL bll = new ContentBLL();
                bool check = bll.CheckType(name);
                if (check)
                {
                    return "success:ok";
                }
                else
                {
                    return "success:nook";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// 删除文章
        /// </summary>
        /// <returns></returns>
        public string DeleteContent()
        {
            try
            {
                int id = Convert.ToInt32(Request["contentId"]);
                ContentBLL bll = new ContentBLL();
                int res = bll.DeleteContent(id);
                if(res > 0)
                {
                    return "success";
                }
                return "failure:删除失败";
            }
            catch (Exception)
            {

                throw;
            }
        }



        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        [UserFilter(true)]
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFilter(true)]
        public string Login(string username,string pwd)
        {
            try
            {
                UserBLL bll = new UserBLL();

                AdminUserModel m = bll.LoginAdmin(username,pwd);
                if (m != null)
                {
                    Session["admin"] = m;
                    return "success";
                }
                return "failure:用户名或密码错误";
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "failure:登录失败";
            }
           
        }
    }
}
