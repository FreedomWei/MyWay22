//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Type
    {
        public Type()
        {
            this.Content = new HashSet<Content>();
        }
    
        public int typeId { get; set; }
        public string TypeName { get; set; }
    
        public virtual ICollection<Content> Content { get; set; }
    }
}
