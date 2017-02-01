using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mldel.Contents
{
    public class HuiFuModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CommentId { get; set; }
        public System.DateTime Time { get; set; }
        public string Text { get; set; }
        public int LouCeng { get; set; }
        public int HuiFuUserId { get; set; }
        public string UserImg { get; set; }
        public string UserName { get; set; }
        public string HuiFuUserName { get; set; }
    }
}
