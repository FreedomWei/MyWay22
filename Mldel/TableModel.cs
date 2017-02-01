using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mldel
{
    public class TableModel<T> where T : class ,new()
    {
        public TableModel() { }
        public TableModel(bool isReturnLst)
        {
            IsReturnLst = isReturnLst;
        }

        public TableModel(int index, int Total, IQueryable<T> sql, bool isReturnLst = false)
        {
            this.index = index;
            this.Total = Total == -1 ? sql.Count() : Total;
            this._sql = sql;
            this.IsReturnLst = isReturnLst;
        }


        public TableModel(int index, int Total, List<T> list, bool isReturnLst = false)
        {

            this.index = index;
            this.Total = Total == -1 ? list.Count() : Total;
            this.IsReturnLst = isReturnLst;
            this.Lst = list;
        }




        /// <summary>
        /// 集合数据
        /// </summary>
        [JsonIgnore]
        public List<T> Lst { get; set; }

        public List<T> GetLst
        {
            get
            {
                if (IsReturnLst) return Lst;
                return null;
            }
        }

        public bool IsReturnLst { get; set; }

        /// <summary>
        /// 表格主体html
        /// </summary>
        public string TableBodyHTML { get; set; }

        /// <summary>
        /// 总条数；使用TableModel(int Total, IQueryable&lt;T&gt; sql)进行实例化，此参数将自动进行赋值
        /// </summary>
        public int Total
        {
            get;
            set;
        }

        /// <summary>
        /// 页码
        /// </summary>
        public int index { get; set; }

        //[JsonIgnore]
        //public IQueryable<T> SetSql
        //{
        //    set
        //    {
        //        _sql = value;
        //    }
        //}
        [JsonIgnore]
        private IQueryable<T> _sql;

        // [JsonIgnore]
        //private List<T> _list;
    }
}
