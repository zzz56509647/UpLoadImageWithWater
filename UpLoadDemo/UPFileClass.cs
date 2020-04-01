using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;

namespace UpLoadDemo
{
    /// <summary>
    /// UPFileClass的摘要说明
    /// </summary>
    public class UPFileClass
    {
        //初始化为null的Dictionary的对象
        private static Dictionary<string, string> di = null;
        //声明isSuccess的变量(用于接收执行的方法是否为True或Flase)
        private static bool isSuccess = true;//赋予默认值
        //声明message的变量(用于接收执行的方法返回的信息)
        private static string message = string.Empty;

        #region 生成缩略图片
        /// <summary>
        /// 生成缩略图片
        /// </summary>
        /// <param name="originalImagePath">源图片路径</param>
        /// <param name="thumbnailPath">缩略图片路径</param>
        /// <param name="width">缩略图片的宽度</param>
        /// <param name="height">缩略图片的高度</param>
        /// <param name="mode">缩略方式</param>
        public static Dictionary<string, string> MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            try
            {
                //使用FromFile()方法从指定文件创建Image,参加为物理的路径
                Image originalImage = Image.FromFile(originalImagePath);
                //这里声明指定的高和宽，默认为参数传输进来的
                int towidth = width, toheight = height;
                //声明默认的图片坐标，默认值为0,0
                int x = 0, y = 0;
                //保存源图片的宽度和高度,赋了默认的源图片的宽度和高度
                int ow = originalImage.Width, oh = originalImage.Height;
                switch (mode.ToUpper())
                {
                    case "HW": //指定高宽缩放(会变形)
                        break;
                    case "W": //指定宽,高按比例
                        //公式为(源文件的高度 乘以 需要缩略的宽度 除以 源文件的宽度)
                        toheight = originalImage.Height * width / originalImage.Width;
                        break;
                    case "H":  //指定高,宽按比例
                        //公式为(源文件的高度 乘以 需要缩略的高度 除以 源文件的高度)
                        towidth = originalImage.Width * height / originalImage.Height;
                        break;
                    case "CUT": //指定高宽裁减(不变形)
                        /**
                         * (如果源图片的宽度 除以 源图片的高度) 大于 (指定的宽度 除以 指定的高度)
                         * */
                        if (((double)originalImage.Width / (double)originalImage.Height) > ((double)towidth / (double)toheight))
                        {
                            //oh = originalImage.Height;
                            //保存图片的高度=(源图片的高度 * 指定的宽度 / 指定的高度
                            ow = originalImage.Height * towidth / toheight;
                            //Y坐标等于0
                            y = 0;
                            //X坐标=(源图片的宽度 - 需要保存图片的宽度) / 2倍
                            x = (originalImage.Width - ow) / 2;
                        }
                        else
                        {
                            //ow = originalImage.Width;
                            //保存图片的宽度=(源图片的宽度 * 指定的高度 / 指定的宽度
                            oh = originalImage.Width * height / towidth;
                            //X坐标等于0
                            x = 0;
                            //Y坐标=(源图片的高度 - 需要保存图片的高度) / 2倍
                            y = (originalImage.Height - oh) / 2;
                        }
                        break;
                    default:
                        break;
                }
                //新建一个bmp图片
                Image bitmap = new Bitmap(towidth, toheight);
                //新建一个画板(图形)，调用FromImage()的方法创建一个Grphics，参加为Image的对象
                Graphics g = Graphics.FromImage(bitmap);
                //设置高质量插值法(此属性获取或者设置与此Graphics关联的插补模式)
                //取值请参考:http://msdn.microsoft.com/zh-cn/library/system.drawing.drawing2d.interpolationmode(v=vs.100).aspx
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //设置高质量，低速度呈现平滑程度(此属性获取此Graphics的呈现质量)
                //取值请参加:http://msdn.microsoft.com/zh-cn/library/z714w2y9(v=vs.100).aspx
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //清空画布并以透明背景色填充
                g.Clear(Color.Transparent);

                //在指定位置并且按指定大小绘制原图片的指定部分
                //此方法在指定位置并且按指定大小绘制指定的Image的指定部分
                /**
                 * 1:要绘制的Image;
                 * 2:指定所绘制图像的位置和大小;
                 * 3:指定Image对象中要绘制的部分;
                 * 4:指定所用的度量单位 
                 * */
                g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);
                try
                {
                    //根据路径获取到FileInfo的对象
                    FileInfo fi = new FileInfo(thumbnailPath);
                    if (fi != null)
                        if (fi.Exists)  //如果该图片的对象已经存在，就先删除,否则，会出现相对应的异常
                            fi.Delete();
                    //以JPG格式保存缩略图片
                    bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //此处，分别释放资源
                    originalImage.Dispose();
                    bitmap.Dispose();
                    g.Dispose();
                }
                message = "生成缩略图片成功..";
            }
            catch (Exception)
            {
                isSuccess = false;
                message = "生成缩略图片失败..";
            }
            di = new Dictionary<string, string>();
            di.Add("Success", isSuccess.ToString());
            di.Add("Message", message);
            return di;
        }
        #endregion

        #region 在图片上增加文字水印
        /// <summary>
        /// 在图片上增加文字水印
        /// </summary>
        /// <param name="path">原服务器图片路径</param>
        /// <param name="path_sy">生成的带文字水印的图片路径</param>
        /// <param name="text">在图片上需要显示的文字</param>
        public static Dictionary<string, string> AddFileWatermark(string path, string path_sy, string text = "CHINA")
        {
            try
            {
                //使用FromFile()方法从指定文件创建Image,参加为物理的路径
                Image image = Image.FromFile(path);
                //新建一个画板(图形)，调用FromImage()的方法创建一个Grphics，参加为Image的对象
                Graphics g = Graphics.FromImage(image);
                //此方法在指定位置并且按指定大小绘制指定的Image的指定部分
                /**
                 * 1:要绘制的Image;
                 * 2:左上角的X坐标;
                 * 3:左上角的Y坐标;
                 * 4:所绘制图片的宽度;
                 * 5:所绘制图像的高度
                 * */
                g.DrawImage(image, 0, 0, image.Width, image.Height);
                //实例化字体对象
                /**
                 * 1:字体的格式
                 * 2:字体的大小
                 * */
                Font f = new Font("Verdana", 16);
                //指定颜色的对象,返回是Brush的对象
                Brush b = new SolidBrush(Color.Blue);
                //在指定位置并且用指定的Brush和Font对象绘制指定的文本字符串
                /**
                 * 1:要绘制的字符串;
                 * 2:定义字符串的文本格式;
                 * 3:所绘制文本的左上角的X坐标
                 * 4:所以制文本的左上角的Y坐标
                 * */
                g.DrawString(text, f, b, 15, 15);
                //释放资源
                g.Dispose();
                //根据路径获取到FileInfo的对象
                FileInfo fi = new FileInfo(path_sy);
                if (fi != null)
                    if (fi.Exists) //如果该图片的对象已经存在，就先删除,否则，会出现相对应的异常
                        fi.Delete();
                image.Save(path_sy);
                //释放资源
                image.Dispose();
                message = "添加文字水印成功";
            }
            catch
            {
                isSuccess = false;
                message = "添加文字水印出现错误..";
            }
            di = new Dictionary<string, string>();
            di.Add("Success", isSuccess.ToString());
            di.Add("Message", message);
            return di;
        }
        #endregion

        #region 在图片上生成图片水印
        /// <summary>
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="path">原服务器图片路径</param>
        /// <param name="path_syp">生成的带图片水印的图片路径</param>
        /// <param name="path_sypf">水印图片路径</param>
        public static Dictionary<string, string> AddFileWatermarkPic(string path, string path_syp, string path_sypf)
        {
            try
            {
                //使用FromFile()方法从指定文件创建Image,参加为物理的路径
                Image image = Image.FromFile(path);
                //使用FromFile()方法从指定文件创建Image,参加为物理的路径,加载需要水印图片
                Image copyImage = Image.FromFile(path_sypf);
                //新建一个画板(图形)，调用FromImage()的方法创建一个Grphics，参加为Image的对象
                Graphics g = Graphics.FromImage(image);
                /**
                 * 此方法在指定位置并且按指定大小绘制指定的Image的指定部分
                 * 
                 * 1:要绘制的Image
                 * 2:指定所绘制图片的位置和大小
                 * 3:左上角的X坐标
                 * 4:左上角的Y坐标
                 * 5:绘制的源图片部分的宽度
                 * 6:绘制的源图片部分的高度
                 * 7:所要绘制的单位
                 * */
                g.DrawImage(copyImage, new Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
                //释放资源
                g.Dispose();
                //根据路径获取到FileInfo的对象
                FileInfo fi = new FileInfo(path_syp);
                if (fi != null)
                    if (fi.Exists)//如果该图片的对象已经存在，就先删除,否则，会出现相对应的异常
                        fi.Delete();
                image.Save(path_syp);
                //释放资源
                image.Dispose();
                message = "添加图片水印成功..";
            }
            catch (Exception)
            {
                isSuccess = false;
                message = "添加图片水印失败..";
            }
            di = new Dictionary<string, string>();
            di.Add("Success", isSuccess.ToString());
            di.Add("Message", message);
            return di;
        }
        #endregion
    }
}