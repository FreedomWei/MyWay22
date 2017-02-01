using MyWay.ErrorLog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MyWay.Controllers
{
    // GET: /ImgProcess/

    public class ImgProcessController : Controller
    {
        //
        // GET: /ss/ImgProcess/

        public string ProcessRequest()
        {
            List<string> lst = new List<string>();
            try
            {
                string webDir = Server.MapPath("~");
                webDir = webDir.Substring(0, webDir.Length - 1);
                string saveDir = string.Format("\\Temp\\{1:yyyy.MM.dd}", webDir, DateTime.Now);
                if (!Directory.Exists(webDir + saveDir))
                {
                    Directory.CreateDirectory(webDir + saveDir);
                }
                Random r = new Random();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    string Extension = Path.GetExtension(file.FileName);
                    string svaePath = string.Empty;
                    do
                    {
                        svaePath = string.Format("{0}\\{1:hhmmss}-{2}{3}", saveDir, DateTime.Now, r.Next(100000, 999999), Extension);
                        if (!System.IO.File.Exists(webDir + svaePath)) { break; }
                    } while (true);
                    file.SaveAs(webDir + svaePath);
                    lst.Add(svaePath);
                }
                if (lst.Count == 0)
                {
                    return "failure:没有上传的文件";
                }
                else if (lst.Count == 1)
                {
                    return "success:" + lst[0].Replace("\\", "/");
                }
                else
                {
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    StringBuilder sb = new StringBuilder();
                    jsonSerializer.Serialize(lst, sb);
                    string result = sb.ToString();
                    result = result.Replace("\\", "/");
                    return "success:" + result;
                }
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "failure:上传失败";
            }
        }

        public string ProcessRequest2()
        {
            List<string> lst = new List<string>();
            try
            {
                string goalW = Request["sw"] ?? "640";
                string goalH = Request["sh"] ?? "450";
                string method = Request["method"];
                string webDir = Server.MapPath("~");
                webDir = webDir.Substring(0, webDir.Length - 1);
                string saveDir = string.Format("\\Temp\\{1:yyyy.MM.dd}", webDir, DateTime.Now);
                if (!Directory.Exists(webDir + saveDir))
                {
                    Directory.CreateDirectory(webDir + saveDir);
                }
                Random r = new Random();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    string Extension = Path.GetExtension(file.FileName);

                    string svaePath = string.Empty;
                    do
                    {
                        svaePath = string.Format("{0}\\{1:hhmmss}-{2}{3}", saveDir, DateTime.Now, r.Next(100000, 999999), Extension);
                        if (!System.IO.File.Exists(webDir + svaePath)) { break; }
                    } while (true);
                    string realimgpath = webDir + svaePath;
                    //   Image img = Image.FromFile(realimgpath);
                    if (method == "zoom") //图片缩放
                    {
                        Image img = Image.FromStream(file.InputStream);
                        int w = int.TryParse(goalW, out w) ? w : 600;
                        int h = int.TryParse(goalH, out h) ? h : 450;
                        if (img.Width > w && img.Height > h)
                        {
                            img = GetReducedImage(img, w, h);
                        }
                        if (img == null)
                        {
                            return "failure:图片压缩出错";
                        }
                        // file.SaveAs(realimgpath);
                        img.Save(realimgpath);
                    }
                    else
                    {
                        file.SaveAs(realimgpath);
                    }
                    lst.Add(svaePath);
                }
                if (lst.Count == 0)
                {
                    return "failure:没有上传的文件";
                }
                else if (lst.Count == 1)
                {
                    return "success:" + lst[0].Replace("\\", "/");
                }
                else
                {
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    StringBuilder sb = new StringBuilder();
                    jsonSerializer.Serialize(lst, sb);
                    string result = sb.ToString();
                    result = result.Replace("\\", "/");
                    return "success:" + result;
                }
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return "failure:上传失败";
            }
        }

        /// <summary>
        /// 生成缩略图重载方法1，返回缩略图的Image对象
        /// </summary>
        /// <param name="ResourceImage">传入的原图像</param>
        /// <param name="Width">缩略图的宽度</param>
        /// <param name="Height">缩略图的高度</param>
        /// <returns>缩略图的Image对象</returns>
        public Image GetReducedImage(Image ResourceImage, int Width, int Height)
        {
            try
            {
                //用指定的大小和格式初始化Bitmap类的新实例
                Bitmap bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                //从指定的Image对象创建新Graphics对象
                Graphics graphics = Graphics.FromImage(bitmap);
                //清除整个绘图面并以透明背景色填充
                graphics.Clear(Color.Transparent);
                //在指定位置并且按指定大小绘制原图片对象
                graphics.DrawImage(ResourceImage, new Rectangle(0, 0, Width, Height));
                return bitmap;
            }
            catch (Exception ex)
            {
                Log.WriteFile(ex);
                return null;
            }
        }
    }

}

