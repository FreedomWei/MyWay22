using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWay.ErrorLog
{
    public class WeiException : System.Exception
    {
        Dictionary<string, string> _dic = new Dictionary<string, string>();
        public WeiException(string message, params KVPair[] kvs)
            : base(message)
        {
            _stackTrace = Environment.StackTrace;
            foreach (var item in kvs)
            {
                this[item.Key] = item.Value;
            }
            //this["Message"] = message;
        }

        public override string ToString()
        {
            return Message;
        }



        /// <summary>
        /// 获取调用堆栈上直接帧的字符串表示形式。
        /// 用于描述调用堆栈的直接帧的字符串。
        /// </summary>
        public override string StackTrace
        {
            get { return _stackTrace; }
        }
        string _stackTrace = string.Empty;

        /// <summary>
        /// 获取描述当前异常的消息。
        /// 解释异常原因的错误消息或空字符串 ("")。
        /// </summary>
        //public override string Message
        //{
        //    get { return this["Message"]; }
        //}

        public string this[string key]
        {
            get
            {
                if (_dic.ContainsKey(key))
                {
                    return _dic[key];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (_dic.ContainsKey(key))
                {
                    _dic[key] = value;
                }
                else
                {
                    _dic.Add(key, value);
                }
            }
        }
        public string[] Keys
        {
            get
            {
                return _dic.Keys.ToArray();
            }
        }
    }
}