using BLL;
using Mldel.Contents;
using Mldel.User;
using MyWay.ErrorLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWay.Controllers
{
    public class ContentController : Controller
    {
        //
        // GET: /Content/


        /// <summary>
        /// 新增查看数
        /// </summary>
        /// <returns></returns>
        public string AddSee()
        {
            try
            {
                int cid = Convert.ToInt32(Request["contentId"]);
                ContentBLL bll = new ContentBLL();
                int res = bll.AddSee(cid);
                if (res > 0)
                {
                    return "success";
                }
                return "failure";
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "failure";
            }
        }


        /// <summary>
        /// 用户评论
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public string PingLunTiTiao()
        {
            try
            {
                string text = Request["text"];
                text = text.Replace("<", "&lt;");
                text = text.Replace(">","&gt;");
                int cid = Convert.ToInt32(Request["cid"]);
                int pageIndex = Convert.ToInt32(Request["pageIndex"] ?? "1");
                int pageSize = Convert.ToInt32(Request["pageSize"]?? "5");
                SessionInfo user = Session["sessionInfo"] as SessionInfo;
                ContentBLL bll = new ContentBLL();
                CommentModel model = new CommentModel()
                {
                    ContentId = cid,
                    Text = text,
                    Time = DateTime.Now,
                    UserId = user.userModel.Id,
                    ZhiChi = 0,
                    FanDui = 0
                };
                int res = bll.PingLunTiTiao(model);
                if (res > 1)
                {
                    UserBLL ubll = new UserBLL();

                    ContentPageModel<CommentModel> list = ubll.GetComment(cid,pageIndex,pageSize);
                    list.pageThis = pageIndex;
                    return "success:"+JsonConvert.SerializeObject(list).ToString();
                }
                return "failure:评论失败....";
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "failure:评论出错....";
            }
        }



        /// <summary>
        /// 获取评论分页数据
        /// </summary>
        /// <returns></returns>
        public string GetCommentPageList()
        {
            try
            {
                int cid = Convert.ToInt32(Request["cid"]);
                int pageIndex = Convert.ToInt32(Request["pageIndex"] ?? "1");
                int pageSize = Convert.ToInt32(Request["pageSize"] ?? "5");
                ContentBLL bll = new ContentBLL();

                SessionInfo info = (SessionInfo)Session["sessionInfo"];
                int userid = 0;
                if (info != null)
                {
                    userid = info.userModel.Id;
                }
                ContentPageModel<CommentModel> list = bll.GetCommentPageList(cid, pageIndex, pageSize, userid);
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
        /// 回复评论
        /// </summary>
        /// <returns></returns>
        public string HuiFu()
        {
            try
            {
                ContentBLL bll = new ContentBLL();
                return "success";
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "faliure:失败";
            }
        }


        /// <summary>
        /// 支持评论数
        /// </summary>
        /// <returns></returns>
        public string ZhiChi()
        {
            try
            {
                int commentId = Convert.ToInt32(Request["commentId"]);
                SessionInfo info = (SessionInfo)Session["sessionInfo"];
                ContentBLL bll = new ContentBLL();
                int res = bll.ZhiChi(commentId,info.userModel.Id);
                if (res > 0)
                {
                    return "success:"+res.ToString();
                }
                return "faliure";
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "faliure";
            }
        }

        /// <summary>
        /// 反对评论数
        /// </summary>
        /// <returns></returns>
        public string FanDui()
        {
            try
            {
                int commentId = Convert.ToInt32(Request["commentId"]);
                SessionInfo info = (SessionInfo)Session["sessionInfo"];
                ContentBLL bll = new ContentBLL();
                int res = bll.FanDui(commentId,info.userModel.Id);
                if (res > 0)
                {
                    return "success:" + res.ToString();
                }
                return "faliure";
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "faliure";
            }
        }

    }
}
