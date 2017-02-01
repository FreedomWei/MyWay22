using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWay.Tool
{
    public class Tool
    {
        ///   <summary> 
        ///   移除HTML标签 
        ///   </summary> 
        ///   <param   name="HTMLStr">HTMLStr</param> 
        public static string ParseTags(string HTMLStr)
        {
            return System.Text.RegularExpressions.Regex.Replace(HTMLStr, "<[^>]*>", "");
        }

    }
}
