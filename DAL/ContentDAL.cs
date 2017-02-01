using DB;
using Mldel.Content;
using Mldel.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions; 
namespace DAL
{
    public class ContentDAL
    {
        public int AddSee(int cid)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    var sql = db.Content.Where(c => c.Id == cid).FirstOrDefault();
                    if (sql != null)
                    {
                        sql.See += 1;
                        return db.SaveChanges();
                    }
                    return 0;
                }
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
                using (TransactionScope scope = new TransactionScope())
                {
                    using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                    {
                        var sql1 = db.Comment.Where(c=>c.ContentId == model.ContentId).ToList();
                        int louceng = 1;
                        if (sql1.Count > 0)
                        {
                            if (sql1.Count == 1)
                            {
                                louceng++;
                            }
                            else if(sql1.Count == 2)
                            {
                                louceng += 2;
                            }else if(sql1.Count == 3)
                            {
                                louceng += 3;
                            }
                            else
                            {
                                louceng = (int)sql1[sql1.Count - 1].LouCeng + louceng;
                            }
                        }
                        Comment cm = new Comment()
                        {
                            Text = model.Text,
                            ContentId = model.ContentId,
                            Time = model.Time,
                            UserId = model.UserId,
                            ZhiChi = model.ZhiChi,
                            FanDui = model.FanDui,
                            LouCeng = louceng
                        };
                        //文章表的评论字段加一
                        var sql = db.Content.Where(c => c.Id == model.ContentId).FirstOrDefault();
                        if (sql != null)
                        {
                            sql.CommentNum += 1;
                        }

                        db.Comment.Add(cm);
                        int res = db.SaveChanges();
                        scope.Complete();
                        return res;
                    }
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
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteContent(int id)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    var sql = db.Content.Where(c=>c.Id == id).FirstOrDefault();
                    if (sql != null)
                    {
                        db.Content.Remove(sql);
                    }
                    return db.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// 获取评论数据
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Mldel.Contents.ContentPageModel<Mldel.Contents.CommentModel> GetCommentPageList(int cid, int pageIndex, int pageSize,int userId)
        {
            try
            {
                 using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    ContentPageModel<CommentModel> model = new ContentPageModel<CommentModel>();
                    var sql = from a in db.Comment
                              join b in db.Users on a.UserId equals b.Id
                              where a.ContentId == cid
                              orderby a.LouCeng
                              select new CommentModel()
                              {
                                  //UserId指的是回复的那个用户的
                                  //HuiFuUserId指的是被回复的用户
                                  ContentId = a.ContentId,
                                  Id = a.Id,
                                  Text = a.Text,
                                  Time = a.Time,
                                  UserId = a.UserId,
                                  UserName = b.UserName,
                                  FanDui = a.FanDui,
                                  ZhiChi = a.ZhiChi,
                                  LouCeng = a.LouCeng,
                                  UserImg = b.UserImg
                                  //var customers = DB.Customer.Join(DB.Commission, cst => cst.CommissionId,
                                  //      com => com.CommissionId, (cst, com) => new Customer()
                                  //      {
                                  //          CommissionId = com.CommissionId,
                                  //          CustomerId = cst.CustomerId,
                                  //          CustomerName = cst.CustomerName,
                                  //          ERPCustomerNo = cst.ERPCustomerNo,
                                  //          UserId = cst.UserId
                                  //      }).ToList();


                                  //HuiFuList =  (from h in db.HuiFu                                                
                                  //             join i in db.Users on h.UserId equals i.Id                         
                                  //             where h.UserId == userId                                           
                                  //             orderby h.Id                                                       
                                  //             select new HuiFuModel()                                            
                                  //             {                                                                  
                                  //                 CommentId = h.CommentId,                                       
                                  //                 HuiFuUserId = h.HuiFuUserId,                                   
                                  //                 Id = h.Id,                                                     
                                  //                 LouCeng = h.LouCeng,
                                  //                 Text = h.Text,
                                  //                 Time = h.Time,
                                  //                 UserId = h.UserId,
                                  //                 UserImg = i.UserImg,
                                  //                 UserName = i.UserName,
                                  //                 HuiFuUserName = b.UserName
                                  //             }).ToList()
                              };

                    
                    var list = sql.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();




                    if (userId != 0)
                    {
                        //获取支持反对表
                        var sql1 = db.ZhiChiFanDui.Where(z => z.UserId == userId).ToList();
                        if (sql1.Count > 0)
                        {
                            //循环判断是否有支持或者反对过这条评论
                            //for (int i = 0; i < sql1.Count; i++)
                            //{
                            //    if (list[i].Id == sql1[i].CommentId && list[i].UserId == sql1[i].UserId)
                            //    {
                            //        list[i].IsZhiChiOrFanDui = true;
                            //        list[i].ZhiChiOrFanDui = sql1[i].ZhiOrFanDui;
                            //    }
                            //}
                            foreach (var item in list)
                            {
                                foreach (var item1 in sql1)
                                {
                                    if (item.Id == item1.CommentId && item.UserId == item1.UserId)
                                    {
                                        item.IsZhiChiOrFanDui = true;
                                        item.ZhiChiOrFanDui = item1.ZhiOrFanDui;
                                    }
                                }
                            }
                        }
                    }


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
        /// 修改类别
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public int UpdateType(int id, string name)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    var label = db.Type.Where(l => l.typeId == id).FirstOrDefault();
                    if (label != null)
                    {
                        label.TypeName = name;
                    }
                    return db.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }



        /// <summary>
        /// 新增标签
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int AddLabel(string name)
        {
            try
            {
                    using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                    {
                        Label l = new Label() { LabelName = name };
                        db.Label.Add(l);
                        return db.SaveChanges();
                    }
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// 新增类别
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int AddType(string name)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    DB.Type t = new DB.Type() { TypeName = name };
                    db.Type.Add(t);
                    return db.SaveChanges();
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
        /// <returns></returns>
        public bool CheckLabel(string name)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    var sql = from a in db.Label
                              where a.LabelName==name
                              select a;
                    if (sql.FirstOrDefault() !=null)
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
        /// 修改标签
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public int UpdateLabel(int id, string name)
        {
            try
            {
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    var label = db.Label.Where(l=>l.Id==id).FirstOrDefault();
                    if (label != null)
                    {
                        label.LabelName = name;
                    }
                    return db.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 新增评论支持数
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public int ZhiChi(int commentId,int userId)
        {
            try
            {
                //事务处理
                using (TransactionScope scope = new TransactionScope())
                {
                    using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                    {
                        int num = 0;
                        //判断是否已经支持或者反对过这条评论
                        var sql1 = db.ZhiChiFanDui.Where(z=>z.CommentId==commentId && z.UserId==userId).FirstOrDefault();
                        if (sql1 == null)
                        {
                            //判断评论是否存在，存在则支持数加1
                            var sql = db.Comment.Where(c => c.Id == commentId).FirstOrDefault();
                            if (sql != null)
                            {
                                sql.ZhiChi += 1;
                                num = (int)sql.ZhiChi;
                                //对这条评论支持过后要新增一条支持反对的记录
                                ZhiChiFanDui z = new ZhiChiFanDui() { CommentId = commentId,UserId = userId,ZhiOrFanDui=1};
                                db.ZhiChiFanDui.Add(z);
                                db.SaveChanges();
                                //事务提交
                                scope.Complete();
                                return num;
                            }
                        }
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
        /// 增加评论反对数
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public int FanDui(int commentId,int userId)
        {
            try
            {
                //事务处理
                using (TransactionScope scope = new TransactionScope())
                {
                    using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                    {
                        //判断是否已经支持或者反对过这条评论
                        var sql1 = db.ZhiChiFanDui.Where(z => z.CommentId == commentId && z.UserId == userId).FirstOrDefault();
                        if (sql1 == null)
                        {
                            int num = 0;
                            //判断评论是否存在，存在则反对数加1
                            var sql = db.Comment.Where(c => c.Id == commentId).FirstOrDefault();
                            if (sql != null)
                            {
                                sql.FanDui += 1;
                                num = (int)sql.FanDui;
                                //对这条评论支持过后要新增一条支持反对的记录
                                ZhiChiFanDui z = new ZhiChiFanDui() { CommentId = commentId, UserId = userId ,ZhiOrFanDui = 0};
                                db.ZhiChiFanDui.Add(z);
                                db.SaveChanges();
                                //事务提交
                                scope.Complete();
                                return num;
                            }
                        }
                        return 0;
                    }
                }
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
                using (qds105749277_dbEntities db = new qds105749277_dbEntities())
                {
                    var sql = from a in db.Content
                              join b in db.Label on a.LabelId equals b.Id
                              join c in db.Type on a.typeId equals c.typeId
                              where a.typeId == ContenttypeId
                              select new ContentModel() { 
                              Author = a.Author,
                              Describe = a.Describe,
                              See = a.See,
                              CommentNum = a.CommentNum,
                              CreateTime = a.CreateTime,
                              Id = a.Id,
                              LabelId = a.LabelId,
                              LabelName = b.LabelName,
                              Title = a.Title,
                              TypeNAme = c.TypeName
                              };
                    if (!string.IsNullOrEmpty(search))
                    {
                        sql = sql.Where(s => s.Title.Contains(search) || s.Describe.Contains(search));
                    }
                    count = sql.Count();
                    var list = sql.OrderByDescending(s=>s.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    return list;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
