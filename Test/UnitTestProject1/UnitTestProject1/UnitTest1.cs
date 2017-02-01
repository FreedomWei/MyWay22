using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using MyWay.Areas.WeiXin.Test;
using com.yajingling.Tests.tool;
using System.Net;
using System.Text;

namespace TestWeiXin
{
    [TestClass]
    public class UnitTest1
    {
        //WeixinController target;
        //WeiXinController target;
        Stream inputStream;

        string xmlTextFormat = @"<xml>
    <ToUserName><![CDATA[gh_73a692977497]]></ToUserName>
    <FromUserName><![CDATA[ouNrJt5cGb46daNJJDRgfkSDNIIQ]]></FromUserName>
    <CreateTime>{{0}}</CreateTime>
    <MsgType><![CDATA[text]]></MsgType>
    <Content><![CDATA[{0}]]></Content>
    <MsgId>5832509444155992350</MsgId>
</xml>
";

        string xmlLocationFormat = @"<xml>
  <ToUserName><![CDATA[gh_73a692977497]]></ToUserName>
  <FromUserName><![CDATA[ouNrJt5cGb46daNJJDRgfkSDNIIQ]]></FromUserName>
  <CreateTime>{0}</CreateTime>
  <MsgType><![CDATA[location]]></MsgType>
  <Location_X>31.285774</Location_X>
  <Location_Y>120.597610</Location_Y>
  <Scale>19</Scale>
  <Label><![CDATA[中国江苏省苏州市沧浪区桐泾南路251号-309号]]></Label>
  <MsgId>5832828233808572154</MsgId>
</xml>";


        private string xmlEvent_ClickFormat = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xml>
  <ToUserName><![CDATA[gh_73a692977497]]></ToUserName>
  <FromUserName><![CDATA[ouNrJt5cGb46daNJJDRgfkSDNIIQ]]></FromUserName>
  <CreateTime>{{0}}</CreateTime>
  <MsgType><![CDATA[event]]></MsgType>
  <Event><![CDATA[CLICK]]></Event>
  <EventKey><![CDATA[{0}]]></EventKey>
</xml>
";

        private string xmlVideoFormat = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xml>
  <ToUserName><![CDATA[gh_73a692977497]]></ToUserName>
  <FromUserName><![CDATA[ouNrJt5cGb46daNJJDRgfkSDNIIQ]]></FromUserName>
  <CreateTime>{0}</CreateTime>
  <MsgType><![CDATA[video]]></MsgType>
  <Video>
    <MediaId><![CDATA[mediaId]]></MediaId>
    <ThumbMediaId><![CDATA[thumbMediaId]]></ThumbMediaId>
  </Video> 
</xml>";

        private string xmlImageFormat = @"<xml>
  <ToUserName><![CDATA[gh_73a692977497]]></ToUserName>
  <FromUserName><![CDATA[ouNrJt5cGb46daNJJDRgfkSDNIIQ]]></FromUserName>
  <CreateTime>1425200601</CreateTime>
  <MsgType><![CDATA[image]]></MsgType>
  <PicUrl><![CDATA[http://mmbiz.qpic.cn/mmbiz/ZxBXNzgHyUq9W2782SegwYFwpf9mK9a6GGToC31ZjpJRH4pD4xnMXStxmx9vQbvZPwmJ1kcffz3KyNtGPVDJhw/0]]></PicUrl>
  <MsgId>6121189971737837469</MsgId>
  <MediaId><![CDATA[rS0qWb1wSLNpLjv3gD1QQnZV8WcL29CTqlf9uyC0jj1Nha2Sv4jJ_0LsT88qVe2a]]></MediaId>
</xml>
";


        /// <summary>
        /// 模拟微信发送信息
        /// </summary>
        [TestMethod]
        public void HttpPost()
        {
            //string postDataStr = xmlImageFormat;
            //string postDataStr = string.Format(string.Format(xmlTextFormat, "BB"), DateTimeHelper.GetWeixinDateTime(DateTime.Now));
            string postDataStr = string.Format(string.Format(xmlEvent_ClickFormat, "news"), DateTimeHelper.GetWeixinDateTime(DateTime.Now));
            string Url = "http://localhost:3163/w/weixin/Index?username=测试公众号";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            //request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            //return retString;
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        [TestMethod]
        public void CreaterMune()
        {
            string postDataStr = string.Format(string.Format(xmlTextFormat, "AAAAAAAAAAAAAAAAAAAAAAA"), DateTimeHelper.GetWeixinDateTime(DateTime.Now));
            string Url = "http://localhost:3163/w/weixin/V";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            //request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
        }

    }
}
