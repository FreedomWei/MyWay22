using Moq;
using MyWay.Areas.WeiXin.Meterial;
using MyWay.Areas.WeiXin.WeiXinHellp;
using MyWay.Areas.WeiXin.WeiXinModel;
using MyWay.Areas.WeiXin.WeiXinTool;
using MyWay.ErrorLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Xml;
using System.Xml.Linq;

namespace MyWay.Areas.WeiXin.Controllers
{

    public class WeiXinController : Controller
    {
        //
        // GET: /WeiXin/WeiXin/
        public WeiXinController()
        {

        }

        /// <summary>
        /// 将xml节点CDATA转换成json之后的键名
        /// </summary>
        private const string CDATA_KEY = "#cdata-section";

        const string Token = "cjwFreedom";//你的token 
        public static int expires_in = 0;
        public static string access_token = "";
        //public static string Access_token
        //{
        //    get
        //    {
        //        int time = ConvertDateTimeInt(DateTime.Now);
        //        if (access_token == "" || time > expires_in)
        //        {
        //            AccessApp app = new AccessApp() { appID = "wxbf1387e25455a9e3", appsecret = "a680e1ecaa818a7a25f9516ffe457c48" };
        //            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + app.appID + "&secret=" + app.appsecret;
        //            HttpRequestUtility http = new HttpRequestUtility(url);
        //            string str = http.Response();
        //            if (str.IndexOf("access_token") == -1)
        //            {
        //                access_token = "";
        //            }
        //            else
        //            {
        //                AccessModel model = JsonConvert.DeserializeObject<AccessModel>(str);
        //                access_token = model.access_token;
        //                expires_in = model.expires_in + time - 200;
        //            }
        //            return access_token;
        //        }
        //        WXLog.Write(access_token, "获取的token");
        //        return access_token;
        //    }
        //}


//        public string Index()
//        {
//            try
//            {

//                string http = Request.HttpMethod.ToLower();
//                if (http == "post")
//                {
//                    WXLog.Write(http, "微信Post请求");
//                    string ToUserName = string.Empty;
//                    string FromUserName = string.Empty;
//                    string CreateTime = string.Empty;
//                    string MsgType = string.Empty;
//                    string sToken = "cjwFreedom";
//                    string sAppID = "wx1d85b4b8b1daa55c";
//                    string sEncodingAESKey = "45oa1QcJZ5tcmDEA6N1BzJLmHasol8tfPKEAlnj5bVv";
//                    WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);
//                    string sReqData = GetPost();
//                    string sReqMsgSig = Request["msg_signature"];
//                    string sReqTimeStamp = Request["timestamp"];
//                    string sReqNonce = Request["nonce"];
//                    string sMsg = "";  //解析之后的明文
//                    int ret = 0;
//                    string xml;
//                    string resultXML = "";
//                    string res = "";
//                    string MsgId = "";
//                    ret = wxcpt.DecryptMsg(sReqMsgSig, sReqTimeStamp, sReqNonce, sReqData, ref sMsg);
//                    WXLog.Write(ret.ToString(), "解密后的结果代码");
//                    if (ret != 0)
//                    {
//                        //解密出错
//                        WXLog.Write("错误代码：" + ret.ToString() + "\r\nAppId:" + sReqMsgSig, "推送事件解密出错");
//                        return "错误代码：" + ret.ToString();
//                    }
//                    try
//                    {
//                        var xDoc = XDocument.Parse(sMsg);
//                        var q = (from c in xDoc.Elements() select c).ToList();
//                        ToUserName = q.Elements("ToUserName").First().Value.ToString();//gzh
//                        FromUserName = q.Elements("FromUserName").First().Value.ToString();//openid
//                        CreateTime = q.Elements("CreateTime").First().Value.ToString();
//                        MsgType = q.Elements("MsgType").First().Value.ToString();
//                        WXLog.Write(MsgType, "MsgType消息类型" + MsgType);
                     
//                        switch (MsgType)
//                        {
//                            case "text":
//                                MsgId = q.Elements("MsgId").First().Value;
//                                string text = q.Elements("Content").First().Value;
//                                WXLog.Write(text, "文本解析");
//                                if (text == "时间")
//                                {
//                                    res = "当前时间是：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
//                                }
//                                else if (text == "天气")
//                                {
//                                    //string url = "http://api.map.baidu.com/telematics/v3/weather?location=" + text + "&output=json&ak=UqrGY8Gd5570LDCgG9NHttyu";
//                                    //HttpRequestUtility http = new HttpRequestUtility(url);
//                                    //string res = http.Response();
//                                    //WXLog.Write(res,"JSON数据");
//                                    res = "没有天气预报";
//                                }
//                                else
//                                {
//                                    res = "你发送的是:" + text;
//                                }
//                                break;
//                            case "image":
//                                MsgId = q.Elements("MsgId").First().Value;
//                                res = "暂时不支持回复图片";
//                                break;
//                            case "voice":
//                                //语音事件
//                                MsgId = q.Elements("MsgId").First().Value;
//                                string Recognition = q.Elements("Recognition").First().Value;
//                                WXLog.Write(Recognition, "语音解析");
//                                res = "你说的是：" + Recognition;
//                                break;
//                            case "event":
//                                ////事件类型
//                                WXLog.Write("", "进来事件了");
//                                string Event = q.Elements("Event").First().Value;
//                                WXLog.Write(Event, "事件类型");
//                                switch (Event)
//                                {
//                                    case "subscribe":
//                                        WXLog.Write(Event, "进来关注事件了");
//                                        //关注事件
//                                        res = "欢迎关注第一维的订阅号";
//                                        break;
//                                    case "unsubscribe":
//                                        //取消关注
//                                        WXLog.Write(Event, "进来取消关注事件了" + Event);
//                                        break;
//                                    case "location":
//                                        //取消关注
//                                        WXLog.Write(Event, "进来地理位置事件了" + Event);
//                                        break;
//                                    default:
//                                        WXLog.Write(Event, "什么事件类型都没有了");
//                                        break;
//                                }
//                                break;
//                            default:
//                                res = "好像报错了";
//                                break;
//                        }
//                        if (MsgType == "location")
//                        {
//                            //resultXML = "<MsgType><![CDATA[image]]>< MsgType><Image><MediaId>< ![CDATA["++"]]></MediaId></Image>";
//                            //resultXML = "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + res + "]]></Content>";
//                            resultXML = "<Latitude>23.137466</Latitude><Longitude>113.352425</Longitude><Precision>119.385040</Precision>";
//                        }
//                        else
//                        {
//                            resultXML = "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + res + "]]></Content>";
//                        }
//                        string _xml = @"<xml>
//                                   <ToUserName><![CDATA[{0}]]></ToUserName>
//                                   <FromUserName><![CDATA[{1}]]></FromUserName>
//                                   <CreateTime>{2}</CreateTime>
//                                   {3}
//                                </xml>";
//                        if (resultXML == "")
//                        {
//                            return "success";
//                        }
//                        xml = string.Format(_xml, FromUserName, ToUserName, CreateTime, resultXML);
//                        WXLog.Write(xml, "解析好的xml");
//                        string sEncryptMsg = ""; //xml格式的密文
//                        ret = wxcpt.EncryptMsg(xml, sReqTimeStamp, sReqNonce, ref sEncryptMsg);
//                        WXLog.Write(sEncryptMsg, "xml格式的密文返回的结果");
//                        return sEncryptMsg;
//                    }
//                    catch (Exception e)
//                    {
//                        WXLog.Write(e.Message, "异常error");
//                        Log.WriteFile(e);
//                        return "error";
//                    }
//                }
//                else
//                {
//                    #region 微信公众号接入
//                    string signature = Request.QueryString["signature"];
//                    string timestamp = Request.QueryString["timestamp"];
//                    string nonce = Request.QueryString["nonce"];
//                    string echostr = Request.QueryString["echostr"];
//                    Request.SaveAs(MvcApplication.DirParent + "微信公众号接入.txt", true);
//                    if (checkSignature(signature, timestamp, nonce))
//                    {
//                        return echostr;
//                    }
//                    #endregion 微信公众号接入 end
//                }
//                return "success:";
//            }
//            catch (Exception ex)
//            {
//                WXLog.Write(ex.Message, "微信错误信息.txt");
//                return "出现异常";
//            }
//        }

        public ActionResult Te()
        {
            return View();
        }


        public void Menu()
        {
            //string userName = lbPublicAccount.SelectedValue;
            string userName = "测试公众号";
            MenuContainer mc1 = MenuHelper.CreateContainer("Freedom");
            mc1.Add(MenuHelper.CreateItem(MenuTypeEnum.click, "关于", "about"));
            mc1.Add(MenuHelper.CreateItem(MenuTypeEnum.view, "博客", "http://www.chenjinwei.com"));
            mc1.Add(MenuHelper.CreateItem(MenuTypeEnum.location_select, "方位", "location"));
            mc1.Add(MenuHelper.CreateItem(MenuTypeEnum.view, "网页授权", OAuthAccessToken.GetOAuthUrl(userName, "http://www.chenjinwei.com/w", OAuthScopeEnum.snsapi_userinfo, "oauth")));
            MenuContainer mc2 = MenuHelper.CreateContainer("Hope");
            mc2.Add(MenuHelper.CreateItem(MenuTypeEnum.scancode_push, "扫码推", "push"));
            mc2.Add(MenuHelper.CreateItem(MenuTypeEnum.click, "图文", "news"));
            mc2.Add(MenuHelper.CreateItem(MenuTypeEnum.scancode_waitmsg, "接收扫码", "waitmsg"));
            MenuContainer mc3 = MenuHelper.CreateContainer("Justice");
            mc3.Add(MenuHelper.CreateItem(MenuTypeEnum.pic_sysphoto, "系统发图", "sysphoto"));
            mc3.Add(MenuHelper.CreateItem(MenuTypeEnum.pic_photo_or_album, "拍照或相册发图", "photo_or_album"));
            mc3.Add(MenuHelper.CreateItem(MenuTypeEnum.pic_weixin, "微信发图", "weixin"));
            ErrorMessage errorMessage = MenuHelper.Create(userName, new BaseMenu[] { mc1, mc2, mc3 });
        }


//        public string Index()
//        {
//            try
//            {
//                string http = Request.HttpMethod.ToLower();
//                if (http == "post")
//                {
//                    Menu();
//                    string ToUserName = string.Empty;
//                    string FromUserName = string.Empty;
//                    string CreateTime = string.Empty;
//                    string MsgType = string.Empty;
//                    string xml;
//                    string resultXML = "";
//                    string res = "";
//                    string MsgId = "";
//                    string sReqData = GetPost();
//                    try
//                    {
//                        var xDoc = XDocument.Parse(sReqData);
//                        var q = (from c in xDoc.Elements() select c).ToList();
//                        ToUserName = q.Elements("ToUserName").First().Value.ToString();//gzh
//                        FromUserName = q.Elements("FromUserName").First().Value.ToString();//openid
//                        CreateTime = q.Elements("CreateTime").First().Value.ToString();
//                        MsgType = q.Elements("MsgType").First().Value.ToString();
//                        WXLog.Write(MsgType, "MsgType消息类型" + MsgType);
//                        switch (MsgType)
//                        {
//                            case "event":
//                                //事件类型
//                                string Event = q.Elements("Event").First().Value.ToString();
//                                WXLog.Write(Event, "事件类型" + Event);
//                                switch (Event)
//                                {
//                                    case "subscribe":
//                                        WXLog.Write(Event, "进来关注事件了");
//                                        //关注事件
//                                        res = "欢迎关注订阅号";
//                                        resultXML = "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + res + "]]></Content>";
//                                        break;
//                                    case "unsubscribe":
//                                        //取消关注
//                                        WXLog.Write(Event, "进来取消关注事件了");
//                                        break;
//                                    default:
//                                        break;
//                                }
//                                break;
//                            case "location":
//                                //取消关注
//                                string lon = q.Elements("Location_X").First().Value;//地理经度
//                                string lat = q.Elements("Location_Y").First().Value;//地理纬度
//                                string Scale = q.Elements("Scale").First().Value;
//                                string label = q.Elements("Label").First().Value;//地理精度
//                                WXLog.Write("经度：" + lon + "维度：" + lat + "精度：" + label + "Scale" + Scale, "进来位置事件了");
//                                break;
//                            case "text":
//                                //文本消息
//                                MsgId = q.Elements("MsgId").First().Value;
//                                string text = q.Elements("Content").First().Value;
//                                WXLog.Write(text, "文本解析");
//                                if (text == "时间")
//                                {
//                                    res = "当前时间是：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
//                                }
//                                else if (text == "天气")
//                                {
//                                    //string url = "http://api.map.baidu.com/telematics/v3/weather?location=" + text + "&output=json&ak=UqrGY8Gd5570LDCgG9NHttyu";
//                                    //HttpRequestUtility http = new HttpRequestUtility(url);
//                                    //string res = http.Response();
//                                    WXLog.Write(res, "JSON数据");
//                                    res = "没有天气预报";
//                                }
//                                else
//                                {
//                                    res = "你发送的是:" + text;
//                                }
//                                //resultXML = "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + res + "]]></Content>";
//                                //resultXML = "<MsgType><![CDATA[news]]></MsgType><ArticleCount>2</ArticleCount><Articles><item><Title><![CDATA[这是图文标题]]></Title> <Description><![CDATA[这是图文描述]]></Description><PicUrl><![CDATA[http://mmbiz.qpic.cn/mmbiz/ZxBXNzgHyUq9W2782SegwYFwpf9mK9a6GGToC31ZjpJRH4pD4xnMXStxmx9vQbvZPwmJ1kcffz3KyNtGPVDJhw/0]]></PicUrl><Url><![CDATA[http://www.chenjinwei.com]]></Url></item><item><Title><![CDATA[第二个标题]]></Title><Description><![CDATA[第二个描述]]></Description><PicUrl><![CDATA[http://mmbiz.qpic.cn/mmbiz/ZxBXNzgHyUq9W2782SegwYFwpf9mK9a6GGToC31ZjpJRH4pD4xnMXStxmx9vQbvZPwmJ1kcffz3KyNtGPVDJhw/0]]></PicUrl><Url><![CDATA[http://www.chenjinwei.com]]></Url></item></Articles>";


//                                resultXML = "<MsgType><![CDATA[music]]></MsgType><Music><Title><![CDATA[音乐标题]]></Title><Description><![CDATA[天籁之音]]></Description><MusicUrl><![CDATA[http://play.baidu.com/?__m=mboxCtrl.playSong&__a=124071745&__o=song/124071745||playBtn&fr=ps||www.baidu.com#]]></MusicUrl><HQMusicUrl><![CDATA[http://play.baidu.com/?__m=mboxCtrl.playSong&__a=124071745&__o=song/124071745||playBtn&fr=ps||www.baidu.com#]]></HQMusicUrl></Music>";

//                                break;
//                            case "image":
//                                //MsgId = q.Elements("MsgId").First().Value;
//                                string url = q.Elements("PicUrl").First().Value;

//                                res = "暂时不支持回复图片";
//                                resultXML = "<MsgType><![CDATA[news]]></MsgType><ArticleCount>2</ArticleCount><Articles><item><Title><![CDATA[这是图文标题]]></Title> <Description><![CDATA[这是图文描述]]></Description><PicUrl><![CDATA["+url+"]]></PicUrl><Url><![CDATA[http://www.chenjinwei.com]]></Url></item><item><Title><![CDATA[第二个标题]]></Title><Description><![CDATA[第二个描述]]></Description><PicUrl><![CDATA["+url+"]]></PicUrl><Url><![CDATA[http://www.chenjinwei.com]]></Url></item></Articles>";
//                                break;
//                            case "voice":
//                                MsgId = q.Elements("MsgId").First().Value;
//                                //语音事件
//                                string Recognition = q.Elements("Recognition").First().Value;
//                                WXLog.Write(Recognition, "语音解析");
//                                res = "你说的是：" + Recognition;
//                                resultXML = "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + res + "]]></Content>";
//                                break;
//                            default:
//                                break;
//                        }
//                        string _xml = @"<xml>
//                                   <ToUserName><![CDATA[{0}]]></ToUserName>
//                                   <FromUserName><![CDATA[{1}]]></FromUserName>
//                                   <CreateTime>{2}</CreateTime>
//                                   {3}
//                                </xml>";
//                        if (resultXML == "")
//                        {
//                            resultXML = "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[什么事件都没触发]]></Content>";
//                        }
//                        xml = string.Format(_xml, FromUserName, ToUserName, CreateTime, resultXML);
//                        WXLog.Write(xml, "解析好的xml");
//                        return xml;
//                    }
//                    catch (Exception e)
//                    {
//                        WXLog.Write(e.Message, "出现异常error");
//                        Log.WriteFile(e);
//                        resultXML = "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[出现错误了！！！！！！！！！！！！]]></Content>";
//                        string _xml = @"<xml>
//                                   <ToUserName><![CDATA[{0}]]></ToUserName>
//                                   <FromUserName><![CDATA[{1}]]></FromUserName>
//                                   <CreateTime>{2}</CreateTime>
//                                   {3}
//                                </xml>";
//                        xml = string.Format(_xml, FromUserName, ToUserName, CreateTime, resultXML);
//                        return xml;
//                    }
//                }
//                else
//                {
//                    #region 微信公众号接入
//                    string signature = Request.QueryString["signature"];
//                    string timestamp = Request.QueryString["timestamp"];
//                    string nonce = Request.QueryString["nonce"];
//                    string echostr = Request.QueryString["echostr"];
//                    Request.SaveAs(MvcApplication.DirParent + "微信公众号接入.txt", true);
//                    if (checkSignature(signature, timestamp, nonce))
//                    {
//                        return echostr;
//                    }
//                    #endregion 微信公众号接入 end
//                }
//                return "success:";
//            }
//            catch (Exception ex)
//            {
//                WXLog.Write(ex.Message, "微信错误信息.txt");
//                return "出现异常";
//            }
//        }


        [NonAction]
        public string GetPost()
        {
            try
            {
                System.IO.Stream s = Request.InputStream;
                StreamReader reader = new StreamReader(s);
                string xmlData = reader.ReadToEnd();
                return xmlData;
            }
            catch (Exception ex)
            {
                WXLog.Write(ex.Message, "GetPost错误");
                throw ex;
            }
        }

        /// <summary>
        /// 公众号接入验证部分
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        public bool checkSignature(string signature, string timestamp, string nonce)
        {
            string[] arr = { "cjwFreedom", timestamp, nonce };
            Array.Sort(arr);
            StringBuilder sb = new StringBuilder();
            foreach (var item in arr)
            {
                sb.Append(item);
            }
            var a = sb.ToString();
            string b = FormsAuthentication.HashPasswordForStoringInConfigFile(a, "SHA1");
            WXLog.Write(b, "微信解密结果.txt");
            return b.ToLower() == signature.ToLower();
        }

        /// <summary>
        /// 从xml字符串解析消息
        /// </summary>
        /// <param name="xml">xml字符串</param>
        /// <returns>返回消息</returns>
        public static RequestBaseMessage Parse(string xml)
        {
            RequestBaseMessage msg = null;
            //将xml字符串解析成JObject对象
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string json = JsonConvert.SerializeXmlNode(doc);
            JObject jo = (JObject)JObject.Parse(json)["xml"];
            //解析消息基类
            msg = ParseBaseMessage(jo);
            //获取各分类的字段，并构造消息
            switch (msg.MsgType)
            {
                case RequestMessageTypeEnum.text:
                    msg = ParseTextMessage(msg, jo);
                    break;
                case RequestMessageTypeEnum.image:
                    msg = ParseImageMessage(msg, jo);
                    break;
                case RequestMessageTypeEnum.voice:
                    msg = ParseVoiceMessage(msg, jo);
                    break;
                //case RequestMessageTypeEnum.video:
                //    msg = ParseVideoMessage(msg, jo);
                //    break;
                //case RequestMessageTypeEnum.location:
                //    msg = ParseLocationMessage(msg, jo);
                //    break;
                //case RequestMessageTypeEnum.link:
                //    msg = ParseLinkMessage(msg, jo);
                //    break;
                case RequestMessageTypeEnum.Event:
                    msg = ParseEventMessage(msg, jo);
                    break;
                default:
                    throw new NotImplementedException(string.Format("未实现消息类型{0:g}解析。", msg.MsgType));
            }
            //返回
            return msg;
        }


        /// <summary>
        /// 解析事件消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回事件消息</returns>
        private static RequestBaseMessage ParseEventMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            RequestBaseMessage msg = null;
            string strEvent = (string)jo["Event"][CDATA_KEY];
            if (string.IsNullOrWhiteSpace(strEvent))
                throw new ArgumentNullException("Event", "Event为空。");
            RequestEventTypeEnum eventType = (RequestEventTypeEnum)Enum.Parse(typeof(RequestEventTypeEnum), strEvent, true);
            switch (eventType)
            {
                case RequestEventTypeEnum.subscribe:
                    msg = ParseSubscribeMessage(baseMessage, jo);
                    break;
                case RequestEventTypeEnum.unsubscribe:
                    msg = ParseUnsubscribeMessage(baseMessage, jo);
                    break;
                //case RequestEventTypeEnum.SCAN:
                //    msg = ParseScanMessage(baseMessage, jo);
                //    break;
                //case RequestEventTypeEnum.LOCATION:
                //    msg = ParseReportLocationMessage(baseMessage, jo);
                //    break;
                //case RequestEventTypeEnum.CLICK:
                //    msg = ParseClickMessage(baseMessage, jo);
                //    break;
                //case RequestEventTypeEnum.VIEW:
                //    msg = ParseViewMessage(baseMessage, jo);
                //    break;
                //case RequestEventTypeEnum.scancode_push:
                //case RequestEventTypeEnum.scancode_waitmsg:
                //    msg = ParseScanCodeMessage(baseMessage, eventType, jo);
                //    break;
                //case RequestEventTypeEnum.pic_sysphoto:
                //case RequestEventTypeEnum.pic_photo_or_album:
                //case RequestEventTypeEnum.pic_weixin:
                //    msg = ParsePicMessage(baseMessage, eventType, jo);
                //    break;
                //case RequestEventTypeEnum.location_select:
                //    msg = ParseLocationSelectMessage(baseMessage, jo);
                //    break;
                //case RequestEventTypeEnum.MASSSENDJOBFINISH:
                //    msg = ParseMassSendJobFinishMessage(baseMessage, jo);
                //    break;
                //case RequestEventTypeEnum.TEMPLATESENDJOBFINISH:
                //    msg = ParseTemplateSendJobFinishMessage(baseMessage, jo);
                //    break;
                //case RequestEventTypeEnum.kf_create_session:
                //    msg = ParseKfCreateAccountMessage(baseMessage, jo);
                //    break;
                //case RequestEventTypeEnum.kf_close_session:
                //    msg = ParseKfCloseAccountMessage(baseMessage, jo);
                //    break;
                //case RequestEventTypeEnum.kf_switch_session:
                //    msg = ParseKfSwitchAccountMessage(baseMessage, jo);
                //    break;
                default:
                    throw new NotImplementedException(string.Format("未实现消息类型{0:g}解析。", msg.MsgType));
            }
            return msg;
        }


        /// <summary>
        /// 解析订阅消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回订阅消息</returns>
        private static RequestSubscribeMessage ParseSubscribeMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            string eventKey = Contains(jo, "EventKey") ? (string)jo["EventKey"][CDATA_KEY] : null;
            string ticket = Contains(jo, "Ticket") ? (string)jo["Ticket"][CDATA_KEY] : null;
            return new RequestSubscribeMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime,
                eventKey, ticket);
        }


        /// <summary>
        /// 解析取消订阅消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回取消订阅消息</returns>
        private static RequestUnsubscribeMessage ParseUnsubscribeMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            return new RequestUnsubscribeMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime);
        }

        /// <summary>
        /// 判断JObject对象是否包含指定的属性
        /// </summary>
        /// <param name="jo">JObject对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>返回是否包含</returns>
        private static bool Contains(JObject jo, string propertyName)
        {
            JToken jt;
            return jo.TryGetValue(propertyName, out jt);
        }



        /// <summary>
        /// 解析消息基类
        /// </summary>
        /// <param name="jo">消息对象</param>
        /// <returns>返回消息基类</returns>
        private static RequestBaseMessage ParseBaseMessage(JObject jo)
        {
            string toUserName, fromUserName, strMsgType;
            DateTime createTime;
            RequestMessageTypeEnum msgType;
            toUserName = (string)jo["ToUserName"][CDATA_KEY];
            fromUserName = (string)jo["FromUserName"][CDATA_KEY];
            createTime = MyWay.Areas.WeiXin.Models.Utility.ToDateTime((int)jo["CreateTime"]);
            strMsgType = (string)jo["MsgType"][CDATA_KEY];
            if (string.IsNullOrWhiteSpace(toUserName))
                throw new ArgumentNullException("ToUserName", "ToUserName为空。");
            if (string.IsNullOrWhiteSpace(fromUserName))
                throw new ArgumentNullException("FromUserName", "FromUserName为空。");
            if (string.IsNullOrWhiteSpace(strMsgType))
                throw new ArgumentNullException("MsgType", "MsgType为空。");
            msgType = (RequestMessageTypeEnum)Enum.Parse(typeof(RequestMessageTypeEnum), strMsgType, true);
            return new RequestBaseMessage(toUserName, fromUserName, createTime, msgType);
        }


        /// <summary>
        /// 解析文本消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回文本消息</returns>
        private static RequestTextMessage ParseTextMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            string content = (string)jo["Content"][CDATA_KEY];
           
            long msgId = (long)jo["MsgId"];
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException("Content", "Content为空。");
            return new RequestTextMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime, content, msgId);
        }

        /// <summary>
        /// 解析图片消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回图片消息</returns>
        private static RequestImageMessage ParseImageMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            string picUrl = (string)jo["PicUrl"][CDATA_KEY];
            string mediaId = (string)jo["MediaId"][CDATA_KEY];
            long msgId = (long)jo["MsgId"];
            if (string.IsNullOrWhiteSpace(picUrl))
                throw new ArgumentNullException("PicUrl", "PicUrl为空。");
            if (string.IsNullOrWhiteSpace(mediaId))
                throw new ArgumentNullException("MediaId", "MediaId为空。");
            return new RequestImageMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime, picUrl, mediaId, msgId);
        }

        /// <summary>
        /// 解析语音消息
        /// </summary>
        /// <param name="baseMessage">消息基类</param>
        /// <param name="jo">消息对象</param>
        /// <returns>返回语音消息</returns>
        private static RequestVoiceMessage ParseVoiceMessage(RequestBaseMessage baseMessage, JObject jo)
        {
            string mediaId = (string)jo["MediaId"][CDATA_KEY];
            string format = (string)jo["Format"][CDATA_KEY];
            string recognition = Contains(jo, "Recognition") ? (string)jo["Recognition"][CDATA_KEY] : null;
            long msgId = (long)jo["MsgId"];
            if (string.IsNullOrWhiteSpace(mediaId))
                throw new ArgumentNullException("MediaId", "MediaId为空。");
            if (string.IsNullOrWhiteSpace(format))
                throw new ArgumentNullException("Format", "Format为空。");
            return new RequestVoiceMessage(baseMessage.ToUserName, baseMessage.FromUserName, baseMessage.CreateTime,
                mediaId, format, recognition, msgId);
        }






        //public string Index()
        //{
        //    try
        //    {
        //        string http = Request.HttpMethod.ToLower();
        //        if (http == "post")
        //        {
        //            //Menu();
        //            string ToUserName = string.Empty;
        //            string FromUserName = string.Empty;
        //            string CreateTime = string.Empty;
        //            string MsgType = string.Empty;
        //            string res = "";
        //            string MsgId = "";
        //            string text = "";
        //            string sReqData = GetPost();
        //            var xDoc = XDocument.Parse(sReqData);
        //            var q = (from c in xDoc.Elements() select c).ToList();
        //            ToUserName = q.Elements("ToUserName").First().Value.ToString();//gzh
        //            FromUserName = q.Elements("FromUserName").First().Value.ToString();//openid
        //            CreateTime = q.Elements("CreateTime").First().Value.ToString();

        //            MsgType = q.Elements("MsgType").First().Value.ToString();
        //            try
        //            {
        //                switch (MsgType)
        //                {
        //                    case "text":
        //                        //回复文本
        //                        //MsgId = q.Elements("MsgId").First().Value;
        //                        text = q.Elements("Content").First().Value;
        //                        WXLog.Write(text, "进来Text事件了");
        //                        if (text == "天气")
        //                        {
        //                            res = "暂不支持天气";
        //                        }
        //                        else if (text == "时间")
        //                        {
        //                            res = "北京时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //                        }
        //                        else
        //                        {
        //                            res = "你发送的是：" + text;
        //                        }
        //                        r = new ResponseTextMessage(ToUserName, FromUserName, DateTime.Now, res);
        //                        break;
        //                    case "image":
        //                        //回复图片
        //                        //MsgId = q.Elements("MsgId").First().Value;
        //                        res = "暂时不支持回复图片";
        //                        WXLog.Write(res, "进来Image事件了");
        //                        r = new ResponseTextMessage(ToUserName, FromUserName, DateTime.Now, res);
        //                        break;
        //                    case "voice":
        //                        //回复语音
        //                        MsgId = q.Elements("MsgId").First().Value;
        //                        text = q.Elements("Recognition").First().Value;
        //                        res = "你说的是：" + text;
        //                        WXLog.Write(res, "进来Voice事件了");
                              
        //                        break;
        //                    case "video":
        //                        //MsgId = q.Elements("MsgId").First().Value;
                               
        //                        break;
        //                    case "news":
        //                        //MsgId = q.Elements("MsgId").First().Value;
        //                        break;
        //                    case "transfer_customer_service":
        //                        break;
        //                    case "music":
        //                        break;
        //                    case "event":
        //                        string Event = q.Elements("Event").First().Value;
        //                        switch (Event)
        //                        {
        //                            case "CLICK":
        //                                res = "你点击了关于";
        //                                WXLog.Write(res, "进来Click事件了");
        //                                r = new ResponseTextMessage(ToUserName, FromUserName, DateTime.Now, res);
        //                                break;
        //                            default:
        //                                break;
        //                        }
        //                        break;
        //                    default:
        //                        break;
        //                }
        //                res = r.ToXml(MessageEncryptTypeEnum.raw);
        //                return res;
        //            }
        //            catch (Exception ex)
        //            {
        //                WXLog.Write(ex.Message, "捕获内层TryCatch异常");

        //                return res;
        //            }
        //        }
        //        else
        //        {
        //            #region 微信公众号接入
        //            string signature = Request.QueryString["signature"];
        //            string timestamp = Request.QueryString["timestamp"];
        //            string nonce = Request.QueryString["nonce"];
        //            string echostr = Request.QueryString["echostr"];
        //            WXLog.Write(echostr, "微信ToKen验证.txt");
        //            if (checkSignature(signature, timestamp, nonce))
        //            {
        //                return echostr;
        //            }
        //            #endregion 微信公众号接入 end
        //        }
        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        WXLog.Write(ex.Message, "捕获外层TryCatch异常");
        //        Log.WriteFile(ex);
        //        return "";
        //    }
        //}


        ///// <summary>
        ///// 处理请求消息，返回响应消息
        ///// </summary>
        ///// <returns>返回响应消息</returns>
        //private ResponseBaseMessage HandleRequestMessage(RequestBaseMessage requestMessage)
        //{
        //    ResponseTextMessage response = new ResponseTextMessage(requestMessage.FromUserName, requestMessage.ToUserName,
        //        DateTime.Now, string.Format("自动回复，请求内容如下：\r\n{0}", requestMessage));
        //    response.Content += "\r\n<a href=\"http://www.chenjinwei.com\">二维也</a>";
        //    ErrorMessage errorMessage = CustomerService.SendMessage(new ResponseTextMessage(requestMessage.FromUserName, requestMessage.ToUserName, DateTime.Now, string.Format("自动回复客服消息，请求内容如下：\r\n{0}", requestMessage.ToString())));
        //    if (!errorMessage.IsSuccess)
        //        Message.Insert(new Message(MessageType.Exception, errorMessage.ToString()));
        //    return response;
        //}


        /// <summary>
        /// 处理请求消息，返回响应消息
        /// </summary>
        /// <returns>返回响应消息</returns>
        private ResponseBaseMessage HandleRequestMessage(RequestBaseMessage requestMessage)
        {
            ResponseTextMessage response = new ResponseTextMessage(requestMessage.FromUserName, requestMessage.ToUserName,
            DateTime.Now, string.Format("自动回复，请求内容如下：\r\n{0}", requestMessage));
            return response;
        }


        public string HandlePost(HttpRequest Request)
        {
            ErrorMessage e = null;
            RequestMessageHelper helper = null;
            BatchMeterial b = MyWay.Areas.WeiXin.Meterial.Meterial.BatchGet("测试公众号", MultiMediaTypeEnum.image, 1, 2, out e);
            try
            {
                string res = "";
                helper = new RequestMessageHelper(Request);
                ResponseBaseMessage responseMessage = null;
                if (helper.Message != null)
                {
                    RequestBaseMessage bm = helper.Message;
                    switch (bm.MsgType)
                    {
                        case RequestMessageTypeEnum.text:
                            WXLog.Write("", "进来" + bm.MsgType + "事件了");
                            ResponseTextMessage ResponseTextMessage = HandleTextMessage((RequestTextMessage)bm);

                            if (ResponseTextMessage.Content == "图文")
                            {
                                WXLog.Write("", "进来图文事件了");
                                string url = "https://mmbiz.qlogo.cn/mmbiz/D1aYATIFatWdxfb99IPptNDj3vqCiaSLpspegmdU4IFpRd40oqctN9gI9g2EZE4qq5eTqEAP2Xjb9J0onD5nNWQ/0?wx_fmt=jpeg";
                                res = "<xml><ToUserName><![CDATA[" + ResponseTextMessage.ToUserName + "]]></ToUserName><FromUserName><![CDATA[" + ResponseTextMessage.FromUserName + "]]></FromUserName><CreateTime>" + ResponseTextMessage.CreateTime + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>2</ArticleCount><Articles><item><Title><![CDATA[第一个标题]]></Title><Description><![CDATA[第一个秒速]]></Description><PicUrl><![CDATA[" + url + "]]></PicUrl><Url><![CDATA[www.chenjinwei.com]]></Url></item><item><Title><![CDATA[第二个标题]]></Title><Description><![CDATA[第二个描述]]></Description><PicUrl><![CDATA[" + url + "]]></PicUrl><Url><![CDATA[chenjinwei.com]]></Url></item></Articles></xml>";
                                WXLog.Write(res, "进来图文事件了res");
                                return res;
                            }


                            res = "<xml><ToUserName><![CDATA[" + ResponseTextMessage.ToUserName + "]]></ToUserName><FromUserName><![CDATA[" + ResponseTextMessage.FromUserName + "]]></FromUserName><CreateTime>" + ResponseTextMessage.CreateTime + "</CreateTime><MsgType><![CDATA[" + ResponseTextMessage.MsgType + "]]></MsgType><Content><![CDATA[你发送的是：" + ResponseTextMessage.Content + "\r\n<a href=\"http://www.chenjinwei.com\">我的博客</a>]]></Content></xml>";
                            return res;
                        case RequestMessageTypeEnum.image:
                            WXLog.Write("", "进来" + bm.MsgType + "事件了");
                            ResponseImageMessage responseImageMessage = HandleImageMessage((RequestImageMessage)bm);
                             res = "<xml><ToUserName><![CDATA[" + responseImageMessage.ToUserName + "]]></ToUserName><FromUserName><![CDATA[" + responseImageMessage.FromUserName + "]]></FromUserName><CreateTime>" + responseImageMessage.CreateTime + "</CreateTime><MsgType><![CDATA[" + responseImageMessage.MsgType + "]]></MsgType><Image><MediaId><![CDATA[" + responseImageMessage.MediaId + "]]></MediaId></Image></xml>";
                            return res;
                        case RequestMessageTypeEnum.voice:
                            WXLog.Write("", "进来" + bm.MsgType + "事件了");
                            ResponseVoiceMessage responseVoiceMessage = HandleVoiceMessage((RequestVoiceMessage)bm);
                            res = "<xml><ToUserName><![CDATA[" + responseVoiceMessage.ToUserName + "]]></ToUserName><FromUserName><![CDATA[" + responseVoiceMessage.FromUserName + "]]></FromUserName><CreateTime>" + responseVoiceMessage.CreateTime + "</CreateTime><MsgType><![CDATA[" + responseVoiceMessage.MsgType + "]]></MsgType><Voice><MediaId><![CDATA[" + responseVoiceMessage.MediaId + "]]></MediaId></Voice></xml>";
                            return res;
                        case RequestMessageTypeEnum.video:
                            WXLog.Write("", "进来" + bm.MsgType + "事件了");
                            responseMessage = HandleRequestMessage(helper.Message);
                            break;
                        case RequestMessageTypeEnum.location:
                            WXLog.Write("", "进来" + bm.MsgType + "事件了");
                            responseMessage = HandleRequestMessage(helper.Message);
                            break;
                        case RequestMessageTypeEnum.link:
                            WXLog.Write("", "进来" + bm.MsgType + "事件了");
                            responseMessage = HandleRequestMessage(helper.Message);
                            break;
                        case RequestMessageTypeEnum.Event:
                            //事件判断
                            WXLog.Write("", "进来" + bm.MsgType + "事件了");
                            RequestEventMessage ev = (RequestEventMessage)bm;
                            switch (ev.Event)
	                        {
                                case RequestEventTypeEnum.subscribe:
                                     //HandleSubscribeMessage((RequestSubscribeMessage)ev);
                                    break;
                                case RequestEventTypeEnum.unsubscribe:
                                    break;
                                case RequestEventTypeEnum.SCAN:
                                    break;
                                case RequestEventTypeEnum.LOCATION:
                                    break;
                                case RequestEventTypeEnum.CLICK:
                                    WXLog.Write("", "进来ev的" + ev.Event + "事件了");
                                    RequestClickMessage click = (RequestClickMessage)ev;
                                    if (click.EventKey == "news")
                                    {
                                        string url = "https://mmbiz.qlogo.cn/mmbiz/D1aYATIFatWdxfb99IPptNDj3vqCiaSLpspegmdU4IFpRd40oqctN9gI9g2EZE4qq5eTqEAP2Xjb9J0onD5nNWQ/0?wx_fmt=jpeg";

                                        WXLog.Write("用户："+bm.ToUserName +"\r\n收件人："+ bm.FromUserName +"\r\n时间"+ bm.CreateTime +"\r\n类型："+ bm.MsgType, click.EventKey);
                                        
                                        res = "<xml><ToUserName><![CDATA[" + bm.ToUserName + "]]></ToUserName><FromUserName><![CDATA[" + bm.FromUserName + "]]></FromUserName><CreateTime>" + bm.CreateTime + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>2</ArticleCount><Articles><item><Title><![CDATA[第一个标题]]></Title><Description><![CDATA[第一个秒速]]></Description><PicUrl><![CDATA[" + url + "]]></PicUrl><Url><![CDATA[www.chenjinwei.com]]></Url></item><item><Title><![CDATA[第二个标题]]></Title><Description><![CDATA[第二个描述]]></Description><PicUrl><![CDATA[" + url + "]]></PicUrl><Url><![CDATA[chenjinwei.com]]></Url></item></Articles></xml>";
                                    }
                                    else if(click.EventKey == "about")
                                    {
                                        WXLog.Write("用户：" + bm.ToUserName + "\r\n收件人：" + bm.FromUserName + "\r\n时间" + bm.CreateTime + "\r\n类型：" + bm.MsgType, click.EventKey);
                                
                                        res = "<xml><ToUserName><![CDATA[" + bm.ToUserName + "]]></ToUserName><FromUserName><![CDATA[" + bm.FromUserName + "]]></FromUserName><CreateTime>" + bm.CreateTime + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[你发送的是：点击了关于]]></Content></xml>";
                                    }
                                    else
                                    {
                                        res = "<xml><ToUserName><![CDATA[" + bm.ToUserName + "]]></ToUserName><FromUserName><![CDATA[" + bm.FromUserName + "]]></FromUserName><CreateTime>" + bm.CreateTime + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[好像没东西\r\n<a href=\"http://www.chenjinwei.com\">我的博客</a>]]></Content></xml>";
                                    }
                                    WXLog.Write(res, "ev的结果");
                                    return res;
                                case RequestEventTypeEnum.VIEW:
                                    break;
                                case RequestEventTypeEnum.scancode_push:
                                    break;
                                case RequestEventTypeEnum.scancode_waitmsg:
                                    break;
                                case RequestEventTypeEnum.pic_sysphoto:
                                    break;
                                case RequestEventTypeEnum.pic_photo_or_album:
                                    break;
                                case RequestEventTypeEnum.pic_weixin:
                                    break;
                                case RequestEventTypeEnum.location_select:
                                    break;
                                case RequestEventTypeEnum.MASSSENDJOBFINISH:
                                    break;
                                case RequestEventTypeEnum.TEMPLATESENDJOBFINISH:
                                    break;
                                case RequestEventTypeEnum.kf_create_session:
                                    break;
                                case RequestEventTypeEnum.kf_close_session:
                                    break;
                                case RequestEventTypeEnum.kf_switch_session:
                                    break;
                                default:
                                    break;
	                        }
                            break;
                        default:
                            break;
                    }
                    WXLog.Write("","来到最后了");
                    return responseMessage.ToXml(helper.EncryptType);
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "<xml><ToUserName><![CDATA[" + helper.Message.ToUserName + "]]></ToUserName><FromUserName><![CDATA[" + helper.Message.FromUserName + "]]></FromUserName><CreateTime>" + helper.Message.CreateTime + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[好像报错了。。。。]]></Content></xml>";
            }
        }


        public ResponseTextMessage HandleTextMessage(RequestTextMessage requestBase)
        {
            ResponseTextMessage response = new ResponseTextMessage(requestBase.FromUserName, requestBase.ToUserName,
               DateTime.Now,requestBase.Content);
            //response.Content += "\r\n<a href=\"http://www.chenjinwei.com\">我的博客</a>";
            return response;
        }

        public ResponseBaseMessage HandleTextMessageByTime(RequestTextMessage requestBase)
        {
            ResponseTextMessage response = new ResponseTextMessage(requestBase.FromUserName, requestBase.ToUserName,
               DateTime.Now, "现在时间"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            response.Content += "\r\n<a href=\"http://www.chenjinwei.com\">我的博客</a>";
            return response;
        }


        public ResponseNewsMessage HandleNewsMessage(RequestClickMessage click)
        {
            string url = "http://mmbiz.qpic.cn/mmbiz/cxGib2YUFqAtzRJWg3v5THk6jTxajRUBneXNxTPByc5waQNR3IlzRickvibEiaG5GjVkibicjqrv1G1wm4zZWVxjKWLQ/0";
            List<Article> list = new List<Article>();
            Article a1 = new Article() { Description = "第一个描述", PicUrl = url, Title = "第一个标题", Url = "www.chenjinwei.com" };
            Article a2 = new Article() { Description = "第二个描述", PicUrl = url, Title = "第二个标题", Url = "www.chenjinwei.com" };
            list.Add(a1);
            list.Add(a2);
            ResponseNewsMessage news = new ResponseNewsMessage(click.ToUserName,click.FromUserName,click.CreateTime,list);
            return news;
        }


        public ResponseVoiceMessage HandleVoiceMessage(RequestVoiceMessage requestBase)
        {
            ResponseVoiceMessage response = new ResponseVoiceMessage(requestBase.FromUserName, requestBase.ToUserName,
               DateTime.Now, requestBase.MediaId);
            return response;
        }


        public ResponseVideoMessage HandleVideoMessage(RequestVideoMessage requestVideoMessage)
        {
            ResponseVideoMessage responseVideo = new ResponseVideoMessage(requestVideoMessage.ToUserName, requestVideoMessage.FromUserName, requestVideoMessage.CreateTime, requestVideoMessage.MediaId);
            return responseVideo;
        }

        public ResponseImageMessage HandleImageMessage(RequestImageMessage requestBase)
        {
            ResponseImageMessage response = new ResponseImageMessage(requestBase.FromUserName, requestBase.ToUserName,
               DateTime.Now,requestBase.MediaId);
            return response;
        }

        /// <summary>
        /// 验证消息的有效性
        /// </summary>
        /// <param name="context"></param>
        /// <returns>如果消息有效，返回true；否则返回false。</returns>
        private bool Validate(HttpContext context)
        {
            //string username = RequestEx.TryGetQueryString("username");  //在接口配置的URL中加入了username参数，表示哪个微信公众号

            AccountInfo account = AccountInfoCollection.GetAccountInfo("测试公众号");
            if (account == null)
                return false;
            string token = account.Token;
            string signature = RequestEx.TryGetQueryString("signature");
            string timestamp = RequestEx.TryGetQueryString("timestamp");
            string nonce = RequestEx.TryGetQueryString("nonce");
            if (string.IsNullOrWhiteSpace(signature) || string.IsNullOrWhiteSpace(timestamp) || string.IsNullOrWhiteSpace(nonce))
                return false;
            return MyWay.Areas.WeiXin.Models.Utility.CheckSignature(signature, token, timestamp, nonce);
        }


        /// <summary>
        /// 处理微信的GET请求，校验签名
        /// </summary>
        /// <param name="context"></param>
        /// <returns>返回echostr</returns>
        private string HandleGet(HttpContext context)
        {
            return RequestEx.TryGetQueryString("echostr");
        }


        public string Index()
        {
            try
            {
                string http = Request.HttpMethod.ToLower();
                HttpRequest hr = HttpContext.ApplicationInstance.Context.Request;
                string result = string.Empty;
                if (http == "get")
                {
                    #region 微信公众号接入
                    string signature = Request.QueryString["signature"];
                    string timestamp = Request.QueryString["timestamp"];
                    string nonce = Request.QueryString["nonce"];
                    string echostr = Request.QueryString["echostr"];
                    WXLog.Write(echostr, "验证echostr");
                    if (checkSignature(signature, timestamp, nonce))
                    {
                        return echostr;
                    }
                    #endregion 微信公众号接入 end
                }
                else if (http == "post")
                {
                    result = HandlePost(hr);
                    WXLog.Write(result, "解析好了");
                }
                return result;

            }
            catch (Exception)
            {
                throw;
            }
        }



        public void V()
        {
            ErrorMessage e = null;
            string url = "www.baidu.com";
            List<MultiMediaArticle> list = new List<MultiMediaArticle>();
            string ThumbMediaId = "nxoGNdQHF7AWPDlpfGZQ9_TkxdCdC3rUe0CbGZ5Ss5Y";
            string Title = "标题";
            string Content = "内容";
            string author = "作者";
            string digest = "摘要";
            bool showCoverPic = false;
            MultiMediaArticle a3 = new MultiMediaArticle(ThumbMediaId, Title, Content, author, url, digest, showCoverPic);
            MultiMediaArticle a4 = new MultiMediaArticle(ThumbMediaId, Title, Content, author, url, digest, showCoverPic);
            list.Add(a3);
            list.Add(a4);
            IEnumerable<MultiMediaArticle> list1 = list.ToList();
            //新增永久图文素材
            //string m = MyWay.Areas.WeiXin.Meterial.Meterial.Add("测试公众号", list1, out e);
            //新增永久图片素材
            //string p = MyWay.Areas.WeiXin.Meterial.Meterial.Add("测试公众号", MultiMediaTypeEnum.image, @"E:/aaa.jpg", out e);
            //获取永久素材列表
            BatchMeterial b = MyWay.Areas.WeiXin.Meterial.Meterial.BatchGet("测试公众号", MultiMediaTypeEnum.image, 0, 2, out e);
            byte[] byt = MyWay.Areas.WeiXin.Meterial.Meterial.Get("测试公众号", "nxoGNdQHF7AWPDlpfGZQ9_TkxdCdC3rUe0CbGZ5Ss5Y",out e);
            string id = b.Item[0].MediaId;
            Response.Write(b.ToString());




            //WebClient wxUpload = new WebClient();
            //string uploadPath = "~/Images/";
            //string folder = Server.MapPath(uploadPath);
            ////自动创建目录
            //if (!Directory.Exists(folder))
            //{
            //    Directory.CreateDirectory(folder);
            //}
            //HttpPostedFile file = Request.Files.Get(0);
            //string filename = folder + file.FileName;
            //file.SaveAs(filename);
            ////API所需的媒体信息
            //wxUpload.Headers.Add("Content-Type", file.ContentType);
            //wxUpload.Headers.Add("filename", file.FileName);
            //wxUpload.Headers.Add("filelength", file.ContentLength.ToString());
            //byte[] result =
            //    wxUpload.UploadFile(
            //        new Uri(string.Format(
            //            "https://api.weixin.qq.com/cgi-bin/media/uploadimg?access_token={0}&type={1}",AnalysisXmlMsg.GetAccessToken("appid", "appsecret"),
            //            "image")), filename);
            //string resultjson = Encoding.UTF8.GetString(result); //在这里获取json数据，获得图片URL


        }



























    }

}
