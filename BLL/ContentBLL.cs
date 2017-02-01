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
    public class ContentBLL
    {

        public int AddSee(int cid)
        {
            try
            {
                ContentDAL dal = new ContentDAL();
                return dal.AddSee(cid);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// 用户评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int PingLunTiTiao(Mldel.Contents.CommentModel model)
        {
            try
            {
                ContentDAL dal = new ContentDAL();
                return dal.PingLunTiTiao(model);
            }
            catch (Exception)
            {
                
                throw;
            }
        }



        public ContentPageModel<Mldel.Contents.CommentModel> GetCommentPageList(int cid, int pageIndex, int pageSize,int userId)
        {
            try
            {
                ContentDAL dal = new ContentDAL();
                return dal.GetCommentPageList(cid,pageIndex,pageSize,userId);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public int ZhiChi(int commentId,int userId)
        {
            try
            {
                ContentDAL dal = new ContentDAL();
                return dal.ZhiChi(commentId,userId);
            }
            catch (Exception)
            {
                
                throw;
            }
        }


        public int FanDui(int commentId,int userId)
        {
            try
            {
                ContentDAL dal = new ContentDAL();
                return dal.FanDui(commentId ,userId);
            }
            catch (Exception)
            {

                throw;
            }
        }



        public List<ContentModel> GetContentList(string search, int pageIndex, int pageSize, int ContenttypeId,out int count)
        {
            try
            {
                ContentDAL dal = new ContentDAL();
                return dal.GetContentList(search,pageIndex,pageSize,ContenttypeId,out count);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// 获取类别
        /// </summary>
        /// <returns></returns>
        public List<TypeModel> GetTyleList()
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    var sql = db.Type.Select(t => new TypeModel() { typeId = t.typeId,TypeName = t.TypeName}).ToList();
                    return sql;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// 删除文章方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteContent(int id)
        {
            try
            {
                ContentDAL dal = new ContentDAL();
                return dal.DeleteContent(id);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 获取类别
        /// </summary>
        /// <param name="search"></param>
        /// <param name="index"></param>
        /// <param name="page"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public TableModel<TypeModel> GetTypes(string search, int index, int page, int total)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    var sql = from a in db.Type
                              where a.TypeName.Contains(search)
                              orderby a.typeId descending
                              select new TypeModel()
                              {
                                  typeId = a.typeId,
                                  TypeName = a.TypeName
                              };
                    TableModel<TypeModel> table = new TableModel<TypeModel>(index, total, sql, true);
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
        /// <summary>
        /// 获取文章类别
        /// </summary>
        /// <param name="search"></param>
        /// <param name="index"></param>
        /// <param name="page"></param>
        /// <param name="total"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public TableModel<ContentModel> GetContentList(string search, int index, int page, int total, int typeId)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {

                    var sql = from a in db.Content
                              join b in db.Label on a.LabelId equals b.Id
                              join c in db.Type on a.typeId equals c.typeId
                              where (a.Title.Contains(search) || a.Text.Contains(search))
                              orderby a.CreateTime descending
                              select new ContentModel()
                              {
                                  Id = a.Id,
                                  Title = a.Title,
                                  See = a.See,
                                  Author = a.Author,
                                  CommentNum = a.CommentNum,
                                  CreateTime = a.CreateTime,
                                  Describe = a.Describe,
                                  ImagePath = a.ImagePath,
                                  LabelId = a.LabelId,
                                  typeId = a.typeId,
                                  TypeNAme = c.TypeName,
                                  LabelName = b.LabelName
                              };
                    if (typeId != 0)
                    {
                        sql = sql.Where(s=>s.typeId == typeId);
                    }
                    TableModel<ContentModel> tableInfo = new TableModel<ContentModel>(index, total, sql, true);
                    var list = sql.Skip(index * page - page).Take(page).ToList();
                    tableInfo.Lst = list;
                    return tableInfo;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改标签
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public int UpdateLabel(int id, string name)
        {
            ContentDAL dal = new ContentDAL();
            return dal.UpdateLabel(id,name);
        }
        /// <summary>
        /// 新增类别
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int AddType(string name)
        {
            ContentDAL dal = new ContentDAL();
            return dal.AddType(name);
        }

        public bool CheckType(string name)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    var sql = from a in db.Type
                              where a.TypeName == name
                              select a;
                    if (sql.FirstOrDefault()!=null)
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
        /// 检查标签是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ChekcLabel(string name)
        {
            ContentDAL dal = new ContentDAL();
            return dal.CheckLabel(name);
        }
        /// <summary>
        /// 新增标签
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int AddLabel(string name)
        {
            ContentDAL dal = new ContentDAL();
            return dal.AddLabel(name);
        }

        /// <summary>
        /// 修改类别
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public int UpdateType(int id, string name)
        {
            ContentDAL dal = new ContentDAL();
            return dal.UpdateType(id,name);
        }

        public ContentModel GetContentById(int id)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    var sql = from a in db.Content
                              where a.Id == id
                              select new ContentModel()
                              {
                                  Id = a.Id,
                                  Title = a.Title,
                                  See = a.See,
                                  Author = a.Author,
                                  CommentNum = a.CommentNum,
                                  CreateTime = a.CreateTime,
                                  Describe = a.Describe,
                                  ImagePath = a.ImagePath,
                                  LabelId = a.LabelId,
                                  typeId = a.typeId,
                                  Text = a.Text
                              };
                    return sql.FirstOrDefault();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
