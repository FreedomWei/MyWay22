﻿using MyWay.Areas.WeiXin.WeiXinHellp;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWay.Areas.WeiXin.WeiXinModel
{
    /// <summary>
    /// 客服的会话状态
    /// </summary>
    public class CustomerServiceSession : IParsable
    {
        /// <summary>
        /// 客户账号
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 会话接入的时间
        /// </summary>
        public int createtime { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomerServiceSession()
        { }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public void Parse(JObject jo)
        {
            openid = (string)jo["openid"];
            createtime = (int)jo["createtime"];
        }

        /// <summary>
        /// 获取接入会话的时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetCreateTime()
        {
            return MyWay.Areas.WeiXin.Models.Utility.ToDateTime(createtime);
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("客户账号：{0}\r\n接入会话的时间：{1}",
                openid, GetCreateTime());
        }
    }
}