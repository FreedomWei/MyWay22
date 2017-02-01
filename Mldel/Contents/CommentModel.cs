using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mldel.Contents
{
    public class CommentModel
    {
        public int Id { get; set; }
         [JsonIgnore]
        public System.DateTime Time { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public int ContentId { get; set; }
        public string  UserName { get; set; }
        public Nullable<int> ZhiChi { get; set; }
        public Nullable<int> FanDui { get; set; }
        public Nullable<int> LouCeng { get; set; }
        public Nullable<int> HuiFuUserId { get; set; }

        public string UserImg { get; set; }
        public List<HuiFuModel> HuiFuList { get; set; }
        public string StringTime
        {
            get
            {
                return Time.ToString("yyyy-MM-dd HH:mm");
            }
        }
        /// <summary>
        /// 标记用户是否已经支持或者反对过这条评论
        /// </summary>
        public bool IsZhiChiOrFanDui { get; set; }
        /// <summary>
        /// 标记用户是支持这条评论还是反对这条评论
        /// </summary>
        public int ZhiChiOrFanDui { get; set; }
        public Nullable<int> HuiFuId { get; set; }
    }
}
