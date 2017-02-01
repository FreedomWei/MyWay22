using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Mldel.Content
{
    public class ContentModel
    {
        public string TypeNAme { get; set; }
        public string LabelName { get; set; }



        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        [JsonIgnore]
        public System.DateTime CreateTime { get; set; }
        public Nullable<int> See { get; set; }
        public Nullable<int> CommentNum { get; set; }
        public int LabelId { get; set; }
        public string ImagePath { get; set; }
        public int typeId { get; set; }
        public string Describe { get; set; }

        public string dateString
        {
            get {
                return CreateTime.ToString("yyyy-MM-dd");
            }
        }
    }
}
