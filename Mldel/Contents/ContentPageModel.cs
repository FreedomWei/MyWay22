using Mldel.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mldel.Contents
{
    public class ContentPageModel<T>
    {
        public int pageSize { get; set; }
        public int Count { get; set; }
        public int page { get; set; }
        public List<T> cList { get; set; }
        public int pageCount { get; set; }

        public int pageThis { get; set; }

    }
}
