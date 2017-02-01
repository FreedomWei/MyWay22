using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MyWay.ErrorLog
{
    public class WXLog
    {
        public static void Write(string content, string filename = null)
        {
            try
            {

                if (string.IsNullOrEmpty(filename))
                {
                    filename = DateTime.Now.Date.ToString() + ".txt";
                }
                if (!filename.Contains(".txt"))
                {
                    filename += ".txt";
                }
                content = "\r\n\r\n时间：" + DateTime.Now.ToString() + "=========================\r\n" + content;
                string path = WangZhanPath.DirSiteFile() + filename;
                string dir = System.IO.Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }


                File.AppendAllText(path, content);
            }
            catch (Exception)
            {

            }
        }
        public static void WriteFile(string type, string content)
        {
            try
            {
                string path = string.Empty;
                if (type == "auto")
                {
                    path = WangZhanPath.DirSiteFile() + "log//wx//auto_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                }
                else if (type == "资质认证成功")
                {
                    path = WangZhanPath.DirSiteFile() + "log//wx//认证事件_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                }
                else if (type == "资质认证失败")
                {
                    path = WangZhanPath.DirSiteFile() + "log//wx//资质认证失败_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                }
                else if (type == "名称认证成功")
                {
                    path = WangZhanPath.DirSiteFile() + "log//wx//名称认证成功_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                }
                else if (type == "名称认证失败")
                {
                    path = WangZhanPath.DirSiteFile() + "log//wx//名称认证失败_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                }
                else if (type == "年审通知")
                {
                    path = WangZhanPath.DirSiteFile() + "log//wx//年审通知_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                }
                else if (type == "认证过期失效通知")
                {
                    path = WangZhanPath.DirSiteFile() + "log//wx//认证过期失效通知_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                }

                string dir = System.IO.Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                File.AppendAllText(path, content);
            }
            catch (Exception)
            {

            }
        }
    }
}