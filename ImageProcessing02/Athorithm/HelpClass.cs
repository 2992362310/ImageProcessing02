using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageProcessing02.Athorithm
{
    public class HelpClass
    {
        public static Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(stream);
                Bitmap bitmap = new Bitmap(stream);

                return new Bitmap(bitmap);
            }
        }

        public static BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);

                stream.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }

        //public static void OpenFile()
        //{
        //    // 创建OpenFileDialog
        //    OpenFileDialog opnDlg = new OpenFileDialog();
        //    //为图像选择一个筛选器
        //    opnDlg.Filter = "所有图像文件|*.bmp;*.pcx;*.png;*.jpg;*.gif;" +
        //        "*.tif;*.ico;*.dxf;*.cgm;*.cdr;*.wmf;*.eps;*.emf|" +
        //        "位图(*.bmp;*.jpg;*.png;...)|*.bmp;*.pcx;*.png;*.jpg;*.gif;*.tif;*.ico|" +
        //        "矢量图(*.wmf;*.eps;*.emf;...)|*.dxf;*.cgm;*.cdr;*.wmf;*.eps;*.emf";
        //    //设置对话框标题
        //    opnDlg.Title = "打开图像文件";
        //    //启用“帮助”按钮
        //    //opnDlg.ShowHelp = true;

        //    //如果结果为“打开”，选定文件
        //    if (opnDlg.ShowDialog() == DialogResult.OK)
        //    {
        //        //读取当前选中的文件名
        //        curFileName = opnDlg.FileName;
        //        //使用Image.FromFile创建图像对象
        //        try
        //        {
        //            curBitmpap = (Bitmap)Image.FromFile(curFileName);
        //        }
        //        catch (Exception exp)
        //        {
        //            MessageBox.Show(exp.Message);
        //        }
        //    }
        //}

        //public static void SaveFile()
        //{
        //    SaveFileDialog
        //}
    }
}
