using DB;
using Mldel;
using Mldel.Content;
using Mldel.Contents;
using Mldel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserDAL
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public UserModel Login(string username,string pwd)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    var sql = (from a in db.Users
                               where a.UserName == username && a.Pwd == pwd
                              select new UserModel()
                              {
                                  Id = a.Id,
                                  Email = a.Email,
                                  Pwd = a.Pwd,
                                  NickName = a.NickName,
                                  UserName = a.UserName
                              }).FirstOrDefault();
                    return sql;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ContentPageModel<ContentModel> GetContentListByType2(int typeId, int pageIndex, int pageSize)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    ContentPageModel<ContentModel> model = new ContentPageModel<ContentModel>();
                    var sql = from a in db.Content
                              join b in db.Type on a.typeId equals b.typeId
                              join c in db.Label on a.LabelId equals c.Id
                              where a.typeId == typeId
                              orderby a.CreateTime descending
                              select new ContentModel()
                              {
                                  Author = a.Author,
                                  CommentNum = a.CommentNum,
                                  CreateTime = a.CreateTime,
                                  Id = a.Id,
                                  ImagePath = a.ImagePath,
                                  LabelId = a.LabelId,
                                  See = a.See,
                                  //Text = a.Text,
                                  Describe = a.Describe,
                                  Title = a.Title,
                                  typeId = a.typeId,
                                  TypeNAme = b.TypeName,
                                  LabelName = c.LabelName
                              };
                    var list = sql.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                    model.cList = list;
                    int count = sql.Count();
                    model.Count = count;
                    model.pageCount = Convert.ToInt32(Math.Ceiling((double)count / (double)pageSize));
                    return model;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="regusername"></param>
        /// <param name="regpassword"></param>
        /// <returns></returns>
        public int Reg(string regusername, string regpassword)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {

                    Users u = new Users() { Email = regusername, Pwd = regpassword, NickName = regusername, UserName = regusername, UserImg = "/images/userImg/default.jpg" };
                    db.Users.Add(u);
                    return db.SaveChanges();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// 检测用户是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool CheckUser(string username)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    var sql = db.Users.Where(u => u.UserName == username).FirstOrDefault();
                    if (sql != null)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// 获取资讯中心列表
        /// </summary>
        /// <returns></returns>
        public List<Mldel.Content.ContentModel> GetList()
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    var sql = from a in db.Content
                              join b in db.Type on a.typeId equals b.typeId
                              join c in db.Label on a.LabelId equals c.Id
                              where a.typeId == 1
                              orderby a.CreateTime descending
                              select new ContentModel()
                              {
                                  Author = a.Author,
                                  CommentNum = a.CommentNum,
                                  CreateTime = a.CreateTime,
                                  Id = a.Id,
                                  ImagePath = a.ImagePath,
                                  LabelId = a.LabelId,
                                  See = a.See,
                                  //Text = a.Text,
                                  Describe = a.Describe,
                                  Title = a.Title,
                                  typeId = a.typeId,
                                  TypeNAme = b.TypeName,
                                  LabelName = c.LabelName
                              };
                    var list = sql.Take(9).ToList();
                    return list;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        /// <summary>
        /// 修改方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateContent(ContentModel model)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities()) {
                    Content con = db.Content.Where(c=>c.Id==model.Id).FirstOrDefault();
                    if (con != null)
                    {
                        con.ImagePath = model.ImagePath;
                        con.LabelId = model.LabelId;
                        con.Text = model.Text;
                        con.typeId = model.typeId;
                        con.Author = model.Author;
                        con.Title = model.Title;
                        db.SaveChanges();
                        return con.Id;
                    }
                    else
                    {
                        return 0;
                    }
                   
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// 根据评论数获取文章
        /// </summary>
        /// <returns></returns>
        public List<ContentModel> GetContentByPingLun()
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    var sql = from a in db.Content
                              join b in db.Type on a.typeId equals b.typeId
                              join c in db.Label on a.LabelId equals c.Id
                              orderby a.CommentNum descending
                              select new ContentModel()
                              {
                                  Author = a.Author,
                                  CommentNum = a.CommentNum,
                                  CreateTime = a.CreateTime,
                                  Id = a.Id,
                                  ImagePath = a.ImagePath,
                                  LabelId = a.LabelId,
                                  See = a.See,
                                  //Text = a.Text,
                                  Describe = a.Describe,
                                  Title = a.Title,
                                  typeId = a.typeId,
                                  TypeNAme = b.TypeName,
                                  LabelName = c.LabelName
                              };
                    var list = sql.Take(7).ToList();
                    return list;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        


        /// <summary>
        /// 根据发表时间获取文章
        /// </summary>
        /// <returns></returns>
        public List<ContentModel> GetContentByTime()
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    var sql = from a in db.Content
                              join b in db.Type on a.typeId equals b.typeId
                              join c in db.Label on a.LabelId equals c.Id
                              orderby a.CreateTime descending
                              select new ContentModel()
                              {
                                  Author = a.Author,
                                  CommentNum = a.CommentNum,
                                  CreateTime = a.CreateTime,
                                  Id = a.Id,
                                  ImagePath = a.ImagePath,
                                  LabelId = a.LabelId,
                                  See = a.See,
                                  //Text = a.Text,
                                  Describe = a.Describe,
                                  Title = a.Title,
                                  typeId = a.typeId,
                                  TypeNAme = b.TypeName,
                                  LabelName = c.LabelName
                              };
                    var list = sql.Take(7).ToList();
                    return list;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }



        /// <summary>
        /// 根据查看数量获取文章
        /// </summary>
        /// <returns></returns>
        public List<ContentModel> GetContentBySee()
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    var sql = from a in db.Content
                              join b in db.Type on a.typeId equals b.typeId
                              join c in db.Label on a.LabelId equals c.Id
                              orderby a.See descending
                              select new ContentModel()
                              {
                                  Author = a.Author,
                                  CommentNum = a.CommentNum,
                                  CreateTime = a.CreateTime,
                                  Id = a.Id,
                                  ImagePath = a.ImagePath,
                                  LabelId = a.LabelId,
                                  See = a.See,
                                  //Text = a.Text,
                                  Describe = a.Describe,
                                  Title = a.Title,
                                  typeId = a.typeId,
                                  TypeNAme = b.TypeName,
                                  LabelName = c.LabelName
                              };
                    var list = sql.Take(7).ToList();
                    return list;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// 根据ID获取文章
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public ContentModel GetContentById(int cid)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    var sql = (from a in db.Content
                               join b in db.Type on a.typeId equals b.typeId
                               join c in db.Label on a.LabelId equals c.Id
                               where a.Id == cid
                               select new ContentModel()
                               {
                                   Author = a.Author,
                                   Id = a.Id,
                                   See = a.See,
                                   CommentNum = a.CommentNum,
                                   CreateTime = a.CreateTime,
                                   ImagePath = a.ImagePath,
                                   Text = a.Text,
                                   Title = a.Title,
                                   TypeNAme = b.TypeName,
                                   LabelName = c.LabelName
                               }).FirstOrDefault();
                    return sql;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }



        /// <summary>
        /// 获取为文章标签
        /// </summary>
        /// <returns></returns>
        public List<Mldel.Contents.LabelModel> GetLabel()
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    var sql = from a in db.Label
                              orderby a.Id
                              select new LabelModel()
                              {
                                  Id = a.Id,
                                  LabelName = a.LabelName
                              };
                    var list = sql.Take(9).ToList();
                    return list;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }



        /// <summary>
        /// 获取文章类型
        /// </summary>
        /// <returns></returns>
        public List<Mldel.Contents.TypeModel> GetTypes()
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    var sql = from a in db.Type
                              select new TypeModel()
                              {
                                  typeId = a.typeId,
                                  TypeName = a.TypeName
                              };
                    var list = sql.ToList();
                    return list;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }




        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddContent(ContentModel model)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    Content c = new Content() { 
                    Author = model.Author,
                    CommentNum = model.CommentNum,
                    CreateTime = model.CreateTime,
                    ImagePath = model.ImagePath,
                    LabelId = model.LabelId,
                    typeId = model.typeId,
                    See = model.See,
                    Text = model.Text,
                    Describe = model.Describe,
                    Title = model.Title
                    };
                    db.Content.Add(c);
                    int res = db.SaveChanges();
                    if (res > 0)
                    {
                        res = c.Id;
                    }
                    return res;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }



        /// <summary>
        /// 根据文章类型获取文章列表
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ContentPageModel<ContentModel> GetContentListByType(int typeId, int pageIndex, int pageSize)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    ContentPageModel<ContentModel> model = new ContentPageModel<ContentModel>();
                    var sql = from a in db.Content
                              join b in db.Type on a.typeId equals b.typeId
                              join c in db.Label on a.LabelId equals c.Id
                              where a.typeId == typeId
                              orderby a.See descending
                              select new ContentModel()
                              {
                                  Author = a.Author,
                                  CommentNum = a.CommentNum,
                                  CreateTime = a.CreateTime,
                                  Id = a.Id,
                                  ImagePath = a.ImagePath,
                                  LabelId = a.LabelId,
                                  See = a.See,
                                  //Text = a.Text,
                                  Describe = a.Describe,
                                  Title = a.Title,
                                  typeId = a.typeId,
                                  TypeNAme = b.TypeName,
                                  LabelName = c.LabelName
                              };
                    var list = sql.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                    model.cList = list;
                    int count = sql.Count();
                    model.Count = count;
                    model.pageCount = Convert.ToInt32(Math.Ceiling((double)count / (double)pageSize));
                    return model;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }


        /// <summary>
        /// 获取总注册人数
        /// </summary>
        /// <returns></returns>
        public int GetUserCount()
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    return db.Users.Count();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ContentPageModel<CommentModel> GetComment(int cid,int pageIndex,int pageSize)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    ContentPageModel<CommentModel> model = new ContentPageModel<CommentModel>();
                    var sql = from a in db.Comment
                              join b in db.Users on a.UserId equals b.Id
                              where a.ContentId == cid
                              orderby a.Time descending
                              select new CommentModel()
                              {
                                  ContentId = a.ContentId,
                                  Id = a.Id,
                                  Text = a.Text,
                                  Time = a.Time,
                                  UserId = a.UserId,
                                  UserName = b.UserName,
                                  ZhiChi = a.ZhiChi,
                                  FanDui = a.FanDui
                              };
                    var list = sql.Skip((pageIndex-1)*pageSize).Take(pageSize).ToList();
                    model.cList = list;
                    model.Count = sql.Count();
                    
                    return model;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public AdminUserModel LoginAdmin(string username, string pwd)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    var sql = db.AdminUser.Where(u => u.UserName == username && u.UserPwd == pwd).Select(u=> new AdminUserModel() {Id = u.Id,UserName = u.UserName,UserPwd = u.UserPwd }).FirstOrDefault();
                    return sql;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
