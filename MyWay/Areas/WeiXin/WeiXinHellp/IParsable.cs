using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWay.Areas.WeiXin.WeiXinHellp
{
    /// <summary>
    /// 解析统计数据
    /// </summary>
    public interface IParsable
    {
        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        void Parse(JObject jo);
    }
}
