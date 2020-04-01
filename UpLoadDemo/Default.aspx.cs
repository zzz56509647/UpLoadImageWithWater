using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace UpLoadDemo
{
    public partial class _Default : System.Web.UI.Page
    {
        /****
         * 功能点:
         * 根据用户所上传的图片进行缩略、添加文字水印和图片水印；
         * 
         * 实现方法:
         * 1:在客户端有TextBox的文本框，是需要进行水印处理的文字;
         * 2:FileUpload是用户需要选择的源文件;
         * 3:Button用于上传事件的处理;
         * 4:还有ID为txtHeight和txtWidth的文本框,用于设置需要缩略图片的宽度和高度;
         * 5:还有5个Image，用于显示图片处理的效果; 
         * 
         * */


        #region 点击上传的事件处理
        /// <summary>
        /// 点击上传的事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpLoad_Click(object sender, EventArgs e)
        {
            string[] extensionArray = new string[] { ".BMP", ".GIF", ".JPG", ".JPEG", ".PNG", ".TIFF" };
            try
            {
                string message = string.Empty;
                //判断是否已经有上传的文件
                if (upLoad.HasFile)
                {
                    //获取上传的图片名称
                    string fileName = this.upLoad.FileName;
                    //把上传的图片保存到FileInfo的对象里面
                    FileInfo fi = new FileInfo(this.upLoad.FileName);
                    //获取上传的图片的扩展名
                    string FileExtension = fi.Extension.ToUpper();
                    //判断上传的图片是否符合图片的格式
                    if (!extensionArray.Contains(FileExtension))
                    {
                        this.divMessage.InnerText = "请上传.JPG、.PNG、GIF的图片文件..";
                        return;
                    }
                    //声明一个临时文件夹的变量
                    string folderPath = "~/Temporary/";
                    //声明存放最终处理好的文件夹变量
                    string newFolderPath = "~/Images/";
                    //获取当明的项目的路径加上文件名称
                    string sourcePath = Server.MapPath(folderPath) + fi.Name;
                    try
                    {
                        //把图片保存到临时的文件夹目录里面
                        this.upLoad.SaveAs(sourcePath);
                        this.divMessage.InnerText = "保存图片成功..";
                        this.divSource.Visible = true;
                        this.imgSource.Width = Unit.Pixel(450);
                        this.imgSource.ImageUrl = folderPath + fi.Name;
                    }
                    catch
                    {
                        this.divMessage.InnerText = "保存图片出现错误..";
                        return;
                    }
                    //获取从客户端转入的高度
                    int h = Convert.ToInt32(this.txtHeight.Text.Trim());
                    //获取从客户端转入的宽度
                    int w = Convert.ToInt32(this.txtWidth.Text.Trim());
                    //获取存放最终文件的物理路径
                    string newPath = Server.MapPath(newFolderPath);
                    /**
                     * 调用UPFileClass的MakeThumbnail()缩略的方法    
                     * 
                     * 第一个参数值为:源文件的物理路径
                     * 第二个参数值为:新的文件物理路径，中间加上"T_"是为了避免文件名冲突
                     * 第三个参数值为:需要缩略图片的高度
                     * 第四个参数值为:需要缩略图片的宽度
                     * 第五个参数值为:缩略图片的方式(HW:指定高宽缩放(会变形)；W:指定宽,高按比例；H:指定高,指定高,宽按比例；CUT:指定高宽裁减(不变形)) 
                     * */
                    Dictionary<string, string> di = UPFileClass.MakeThumbnail(sourcePath, newPath + "T_" + fileName, h, w, "H");
                    if (di["Success"].ToUpper() == "TRUE")
                    {
                        this.divThumbnail.Visible = true;
                        this.imgThumbnail.Width = Unit.Pixel(450);
                        this.imgThumbnail.ImageUrl = newFolderPath + "T_" + fileName;
                    }
                    /**
                     * 
                     * 调用UPFileClass的AddFileWatermark()添加文字水印的方法    
                     * 
                     * 第一个参数值为:源文件的物理路径
                     * 第二个参数值为:新的文件物理路径，中间加上"W_"是为了避免文件名冲突
                     * 第三个参数值为:需要添加显示的水印文字,注:可以不需要,因为在被调用方法的时候，已经赋了默认的值                 
                     * */
                    di = UPFileClass.AddFileWatermark(sourcePath, newPath + "W_" + fileName, this.txtText.Text);
                    if (di["Success"].ToUpper() == "TRUE")
                    {
                        this.divWatermark.Visible = true;
                        this.imgWatermark.Width = Unit.Pixel(450);
                        this.imgWatermark.ImageUrl = newFolderPath + "W_" + fileName;
                    }
                    //声明需添加图片的水印的图片路径
                    string wtermarkPicPath = Server.MapPath("~/WtermarkPic/") + "0df3d7ca7bcb0.jpg";
                    /**
                    * 
                    * 调用UPFileClass的AddFileWatermarkPic()添加文字水印的方法    
                    * 
                    * 第一个参数值为:源文件的物理路径
                    * 第二个参数值为:新的文件物理路径，中间加上"WPic_"是为了避免文件名冲突
                    * 第三个参数值为:需要添加显示的水印的图片物理路径,注:可以不需要               
                    * */
                    di = UPFileClass.AddFileWatermarkPic(sourcePath, newPath + "WPic_" + fileName, wtermarkPicPath);
                    if (di["Success"].ToUpper() == "TRUE")
                    {
                        this.divWatermarkPic.Visible = true;
                        this.imgWatermarkPic.Width = Unit.Pixel(450);
                        this.imgWatermarkPic.ImageUrl = newFolderPath + "WPic_" + fileName;
                    }
                    this.divMessage.InnerText = di["Message"];
                    //最后，删除临时的文件
                    //fi = new FileInfo(sourcePath);
                    //if (fi != null)
                    //    if (fi.Exists)
                    //        fi.Delete();
                }
                else
                {
                    this.divMessage.InnerText = "请上传图片...";
                }
            }
            catch (Exception ex)
            {
                this.divMessage.InnerText = "操作出现异常,消息为:" + ex.Message;
            }
        }
        #endregion
    }
}