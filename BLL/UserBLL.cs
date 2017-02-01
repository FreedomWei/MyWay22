using DAL;
using DB;
using Mldel;
using Mldel.Content;
using Mldel.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UserBLL
    {

        public UserModel Login(string username,string pwd)
        {
            try
            {
                UserDAL dal = new UserDAL();
                return dal.Login(username, pwd);
                
            }
            catch (Exception)
            {
                throw;
            }
        }


        public int Reg(string regusername, string regpassword)
        {
            try
            {
                UserDAL dal = new UserDAL();
                return dal.Reg(regusername,regpassword);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public bool CheckUser(string username)
        {
            try
            {
                UserDAL dal = new UserDAL();
                bool res = dal.CheckUser(username);
                return res;
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
                UserDAL dal = new UserDAL();
                return dal.GetContentListByType2(typeId, pageIndex, pageSize);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Mldel.Content.ContentModel> GetList()
        {
            try
            {
                UserDAL dal = new UserDAL();
                return dal.GetList();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public Mldel.Content.ContentModel GetContentById(int cid)
        {
            try
            {
                UserDAL dal = new UserDAL();
                return dal.GetContentById(cid);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public List<Mldel.Contents.LabelModel> GetLabel()
        {
            try
            {
                UserDAL dal = new UserDAL();
                return dal.GetLabel();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public List<Mldel.Contents.TypeModel> GetTypes()
        {
            try
            {
                UserDAL dal = new UserDAL();
                return dal.GetTypes();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public int AddContent(ContentModel model)
        {
            try
            {
                UserDAL dal = new UserDAL();
                return dal.AddContent(model);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public List<ContentModel> GetContentBySee()
        {
            try
            {
                UserDAL dal = new UserDAL();
                return dal.GetContentBySee();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public TableModel<LabelModel> GetLabels(string search, int index, int page, int total)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    var sql = from a in db.Label
                              where a.LabelName.Contains(search)
                              orderby a.Id
                              select new LabelModel()
                              {
                                  Id = a.Id,
                                  LabelName = a.LabelName
                              };
                    TableModel<LabelModel> table = new TableModel<LabelModel>(index,total,sql,true);
                    var list = sql.Skip(index * page - page).Take(page).ToList();
                    table.Lst = list;
                    return table;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ContentModel> GetContentByTime()
        {
            try
            {
                UserDAL dal = new UserDAL();
                return dal.GetContentByTime();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ContentModel> GetContentByPingLun()
        {
            try
            {
                UserDAL dal = new UserDAL();
                return dal.GetContentByPingLun();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ContentPageModel<ContentModel> GetContentListByType(int typeId,int pageIndex,int pageSize)
        {
            try
            {
                UserDAL dal = new UserDAL();
                return dal.GetContentListByType(typeId, pageIndex, pageSize);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public int GetUserCount()
        {
            try
            {
                UserDAL dal = new UserDAL();
                return dal.GetUserCount();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public ContentPageModel<CommentModel> GetComment(int cid, int pageIndex, int pageSize)
        {
            try
            {
                UserDAL dal = new UserDAL();
                return dal.GetComment(cid,pageIndex,pageSize);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public Mldel.User.AdminUserModel LoginAdmin(string username, string pwd)
        {
            try
            {
                UserDAL dal = new UserDAL();
                return dal.LoginAdmin(username,pwd);
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
                UserDAL dal = new UserDAL();
                return dal.UpdateContent(model);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public ContentPageModel<ContentModel> GetContentListByLabelId(int typeId, int pageIndex, int pageSize)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    ContentPageModel<ContentModel> model = new ContentPageModel<ContentModel>();
                    var sql = from a in db.Content
                              join b in db.Type on a.typeId equals b.typeId
                              join c in db.Label on a.LabelId equals c.Id
                              where a.LabelId == typeId
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
        /// 根据搜索词获取文章
        /// </summary>
        /// <param name="sousuo"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ContentPageModel<ContentModel> GetSouSuoContent(string sousuo, int pageIndex, int pageSize)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    ContentPageModel<ContentModel> model = new ContentPageModel<ContentModel>();
                    var sql = from a in db.Content
                              join b in db.Type on a.typeId equals b.typeId
                              join c in db.Label on a.LabelId equals c.Id
                              where a.Title.Contains(sousuo) || a.Describe.Contains(sousuo)
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
    }
}
