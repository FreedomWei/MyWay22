using BLL;
using Mldel;
using Mldel.Content;
using Mldel.Contents;
using Mldel.User;
using MyWay.ErrorLog;
using MyWay.Tool;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyWay.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        //设置缓存配置（在web.config文件可以修改缓存时间）
        //[OutputCache(CacheProfile = "Long")]
        public ActionResult Index()
        {
            try
            {

                UserBLL bll = new UserBLL();
                if (Request.Cookies["info"] != null)
                {
                    string username = Request.Cookies["info"].Values["username"];
                    string pwd = Request.Cookies["info"].Values["pwd"];
                    SessionInfo sessionInfo = new SessionInfo();
                    sessionInfo.userModel = bll.Login(username, pwd);
                    Session["sessionInfo"] = sessionInfo;
                }

                List<ContentModel> list = bll.GetList();
                ViewBag.clist = list;
                ContentPageModel<ContentModel> list2 = bll.GetContentListByType2(2, 1, 6);
                ViewBag.clist2 = list2.cList;
                ContentPageModel<ContentModel> list3 = bll.GetContentListByType(2, 1, 7);
                ViewBag.clist3 = list3.cList;
                List<ContentModel> ContentBySee = bll.GetContentBySee();
                ViewBag.ContentBySee = ContentBySee;
                List<ContentModel> ContentByPingLun = bll.GetContentByPingLun();
                ViewBag.ContentByPingLun = ContentByPingLun;
                List<ContentModel> ContentByTime = bll.GetContentByTime();
                ViewBag.ContentByTime = ContentByTime;

                //在线用户数
                ViewBag.OnLineUserCount = (int)System.Web.HttpContext.Current.Application["OnLineUserCount"];
                //总用户数
                ViewBag.UserCount = bll.GetUserCount();

                //测试的
                ViewBag.aa = "亲测有效";


                return View();
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return new EmptyResult();
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string Login()
        {
            try
            {
                string username = Request["username"];
                string pwd = Request["pwd"];
                bool isAuto = Convert.ToBoolean(Request["isAuto"]);
                string md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "MD5");
                //是否自动登录
                if (isAuto)
                {
                    //向客户端写入Cookie
                    HttpCookie cookie = new HttpCookie("info"); // 创建一个名为uname的cookie
                    cookie.Expires = DateTime.Now.AddDays(3); // 设置该cookie的有效时间
                    cookie.Values.Add("username", username);
                    cookie.Values.Add("pwd", md5);
                    HttpContext.Response.Cookies.Add(cookie); // 提交cookie
                }
                UserBLL bll = new UserBLL();
                SessionInfo sessionInfo = new SessionInfo();
                sessionInfo.userModel = bll.Login(username, md5);
                //string state = "1";
                if (sessionInfo.userModel == null)
                {
                    var res1 = new { state = "0", username = "" };
                    var json1 = JsonConvert.SerializeObject(res1);
                    return json1.ToString();
                }

                Session["sessionInfo"] = sessionInfo;

                var res = new { state = "1", username = sessionInfo.userModel.UserName };
                var json = JsonConvert.SerializeObject(res);
                return json.ToString();
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "登录失败";
            }
        }


        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string reg()
        {
            try
            {
                string regusername = Request["regusername"];
                string regpassword = Request["pwd"];
                UserBLL bll = new UserBLL();
                bool check = CheckUser(regusername);
                if (check)
                {
                    return "用户名已存在";
                }
                string md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(regpassword, "MD5");
                int res = bll.Reg(regusername, md5);
                if (res > 0)
                {
                    return "1";
                }
                return "注册失败";
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "注册错误";
            }
        }

        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool CheckUser(string username)
        {
            try
            {
                UserBLL bll = new UserBLL();
                bool res = bll.CheckUser(username);
                return res;
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                throw;
            }
        }


        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public string LoginOut()
        {
            try
            {
                Session["sessionInfo"] = null;
                HttpContext.Response.Cookies["info"].Expires = DateTime.Now.AddDays(-1);
                return "1";
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "退出失败";
            }
        }

        /// <summary>
        /// 获取标签
        /// </summary>
        /// <returns></returns>
        public string GetLabel()
        {
            try
            {
                UserBLL bll = new UserBLL();
                List<LabelModel> label = bll.GetLabel();
                var res = JsonConvert.SerializeObject(label);
                return "success:" + res.ToString();
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "获取错误";
            }
        }

        /// <summary>
        /// 根据ID获取文章详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(string id)
        {
            try
            {
                int cid = 0;
                int.TryParse(id, out cid);
                if (cid == 0)
                {
                    return Content("");
                }
                UserBLL bll = new UserBLL();
                ContentBLL cbll = new ContentBLL();
                int addsee = cbll.AddSee(cid);
                ContentModel model = bll.GetContentById(cid);
                if (model == null)
                {
                    return Content("");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return new EmptyResult();
            }

        }


        /// <summary>
        /// 编辑文章
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            return View();
        }

        /// <summary>
        /// 文章列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult ContentList()
        {
                ViewBag.typeId = Convert.ToInt32(Request["typeId"]);
                return View();
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <returns></returns>
        public string GetConentPageList()
        {
            try
            {
                int typeId = Convert.ToInt32(Request["typeId"]);
                int pageIndex = Convert.ToInt32(Request["pageIndex"] ?? "1");
                int pageSize = Convert.ToInt32(Request["pageSize"] ?? "5");
                UserBLL bll = new UserBLL();
                ContentPageModel<ContentModel> list = bll.GetContentListByType(typeId, pageIndex, pageSize);
                list.pageThis = pageIndex;
                var res = JsonConvert.SerializeObject(list);
                return "success:" + res.ToString();

            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "faliure:失败";
            }
        }




        /// <summary>
        /// 截取描述文字取文章前50个字
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetStrNum(string text)
        {
            try
            {
                string text1 = Tool.Tool.ParseTags(text);
                if (text1.Length > 50)
                {
                    text1 = text1.Substring(0, 50);
                }
                text1 = text1 + "...";
                return text1;
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "";
            }
        }


        /// <summary>
        /// 获取标签类型
        /// </summary>
        /// <returns></returns>
        public string GetTypes()
        {
            try
            {
                UserBLL bll = new UserBLL();
                List<LabelModel> label = bll.GetLabel();
                List<TypeModel> type = bll.GetTypes();
                if (label.Count < 0 || type.Count < 0)
                {
                    return "failure:获取失败";
                }
                var json = new { type = type, label = label };
                var res = JsonConvert.SerializeObject(json);
                return "success:" + res.ToString();
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "failure:注册错误";
            }
        }

        /// <summary>
        /// 新增文章
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]//防止危险字符提交
        public string PostData()
        {
            try
            {
                string id = Request["contentId"];
                string title = Request["title"];
                string author = Request["author"];
                string text = Request["text"];
                int type = Convert.ToInt32(Request["type"]);
                int label = Convert.ToInt32(Request["label"]);
                string imagepath = Request["imagepaths"];
                string describe = Tool.Tool.ParseTags(text);
                describe = GetStrNum(describe);

                int contentId = 0;
                int.TryParse(id, out contentId);

                ContentModel model = new ContentModel()
                {
                    Title = title,
                    LabelId = label,
                    Author = author,
                    typeId = type,
                    ImagePath = imagepath,
                    Text = text
                };
                UserBLL bll = new UserBLL();
                if (contentId == 0)
                {

                    model.Describe = describe;
                    model.CommentNum = 0;
                    model.CreateTime = DateTime.Now;
                    int res = bll.AddContent(model);
                    if (res > 0)
                    {
                        return "success:"+res.ToString();
                    }
                }
                else
                {
                    model.Id = contentId;
                    int res = bll.UpdateContent(model);
                    if (res > 0)
                    {
                        return "success:" + res.ToString();
                    }
                }
                return "failure:保存失败";
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "failure:" + ex.Message;
            }
        }



        /// <summary>
        /// 标签云页面
        /// </summary>
        /// <returns></returns>
        public ActionResult LabelYunPage()
        {
            ViewBag.typeId = Convert.ToInt32(Request["labelid"]);
            ViewBag.labelName = Request["labelName"];
            return View();
        }

        /// <summary>
        /// 获取标签云页面列表
        /// </summary>
        /// <returns></returns>
        public string GetLabelPageList()
        {
            try
            {
                int typeId = Convert.ToInt32(Request["typeId"]);
                int pageIndex = Convert.ToInt32(Request["pageIndex"] ?? "1");
                int pageSize = Convert.ToInt32(Request["pageSize"] ?? "5");
                UserBLL bll = new UserBLL();
                ContentPageModel<ContentModel> list = bll.GetContentListByLabelId(typeId, pageIndex, pageSize);
                list.pageThis = pageIndex;
                var res = JsonConvert.SerializeObject(list);
                return "success:" + res.ToString();

            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "faliure:失败";
            }
        }

        /// <summary>
        /// 搜索页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SouSuoPage()
        {
            ViewBag.sousuo = Request["sousuo"];
            return View();
        }
        /// <summary>
        /// 获取搜素的结果
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]//防止危险字符提交
        public string GetSouSuoContent()
        {
            string sousuo = Request["sousuo"];
            int pageIndex = Convert.ToInt32(Request["pageIndex"] ?? "1");
            int pageSize = Convert.ToInt32(Request["pageSize"] ?? "5");
            UserBLL bll = new UserBLL();
            ContentPageModel<ContentModel> list = bll.GetSouSuoContent(sousuo, pageIndex, pageSize);
            list.pageThis = pageIndex;
            var res = JsonConvert.SerializeObject(list);
            return "success:" + res.ToString();
        }


        public string GetWeatherbyCityName()
        {
            try
            {
                string cityName = Request["city"];
                string[] arr = new string[23];
                Weather.WeatherWebService web = new Weather.WeatherWebService();
                arr =  web.getWeatherbyCityName(cityName);
                List<string> list = arr.ToList();
                var res = JsonConvert.SerializeObject(list);
                return "success:" + res.ToString();
            }
            catch(System.TimeoutException ex)
            {
                return "请求超时:"+ex.Message;
            }
            catch (Exception ex)
            {

                return "异常信息:"+ex.Message;
            }
        }




        public ActionResult Tests()
        {
            return View();
        }
    }
}
