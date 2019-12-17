using ImageProcessing02.Athorithm;
using ImageProcessing02.UILayer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ImageProcessing02
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private HiPerfTimer myTimer;
        private BitmapImage image = null;

        public MainWindow()
        {
            InitializeComponent();
            myTimer = new HiPerfTimer();
        }

        //打开图片（已放弃）
        private void OpenImage_btn(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "所有图像文件|*.bmp;*.pcx;*.png;*.jpg;*.gif";
            openFileDialog.Title = "打开图像文件";
            if((bool)openFileDialog.ShowDialog())
            {
                originImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
        
        //文件菜单
        private void FileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = e.Source as MenuItem;
            string commond = mi.Header.ToString();
            if (commond == "打开图片")
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                //openFileDialog.Filter = "所有图像文件|*.bmp;*.pcx;*.png;*.jpg;*.gif";
                //openFileDialog.Title = "打开图像文件";

                openFileDialog.Filter = "所有图像文件|*.bmp;*.pcx;*.png;*.jpg;*.gif;" +
                "*.tif;*.ico;*.dxf;*.cgm;*.cdr;*.wmf;*.eps;*.emf|" +
                "位图(*.bmp;*.jpg;*.png;...)|*.bmp;*.pcx;*.png;*.jpg;*.gif;*.tif;*.ico|" +
                "矢量图(*.wmf;*.eps;*.emf;...)|*.dxf;*.cgm;*.cdr;*.wmf;*.eps;*.emf";
                //设置对话框标题
                openFileDialog.Title = "打开图像文件";

                if ((bool)openFileDialog.ShowDialog())
                {
                    originImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                }
            }
            else if (commond == "另存")
            {
                BitmapImage bitmapImage = targetImage.Source as BitmapImage;
                Bitmap bitmap = HelpClass.BitmapImage2Bitmap(bitmapImage);

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "保存为";
                saveFileDialog.OverwritePrompt = true;
                saveFileDialog.Filter = "BMP文件(*.bmp)|*.bmp|" + "Gif文件(*.gif)|*.gif|" + "JPEG文件(*.jpg)|*.jpg|" + "PNG文件(*.png)|*.png";
                if ((bool)saveFileDialog.ShowDialog())
                {
                    string fileName = saveFileDialog.FileName;
                    string strFileExtn = fileName.Remove(0, fileName.Length - 3);

                    switch (strFileExtn)
                    {
                        case "bmp":
                            bitmap.Save(fileName, ImageFormat.Bmp);
                            break;
                        case "jpg":
                            bitmap.Save(fileName, ImageFormat.Jpeg);
                            break;
                        case "gif":
                            bitmap.Save(fileName, ImageFormat.Gif);
                            break;
                        case "tif":
                            bitmap.Save(fileName, ImageFormat.Tiff);
                            break;
                        case "png":
                            bitmap.Save(fileName, ImageFormat.Png);
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                this.Close();
            }
        }

        //像素处理，进度条未实现
        private void ToolMenuItem_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmapImage = originImage.Source as BitmapImage;

            myTimer.Start();

            MenuItem mi = e.Source as MenuItem;
            string commond = mi.Header.ToString();
            if (commond == "暗角")
            {                
                image = PixelTool.Vignetting(bitmapImage);
            }
            else if(commond == "亮度")
            {
                image = PixelTool.Brightness(bitmapImage);
            }
            else if (commond == "去色")
            {
                image = PixelTool.Decolorize(bitmapImage);
            }
            else if (commond == "浮雕")
            {
                image = PixelTool.Relief(bitmapImage);
            }
            else if (commond == "马赛克")
            {
                image = PixelTool.Mosaic(bitmapImage);
            }
            else if (commond == "扩散")
            {
                image = PixelTool.Biffusion(bitmapImage);
            }

            myTimer.Stop();
            timeBox.Text = myTimer.Duration.ToString("####.##" + "毫秒");

            targetImage.Source = image;
        }

        //内存处理
        private void PixelTool_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmapImage = originImage.Source as BitmapImage;

            MenuItem mi = e.Source as MenuItem;
            string commond = mi.Header.ToString();
            if (commond == "暗角")
            {
                //targetImage.Source = ToolClass.Vignetting(bitmapImage);
            }
            else if (commond == "亮度")
            {
                //targetImage.Source = ToolClass.Brightness(bitmapImage);
            }
            else if (commond == "去色")
            {
                targetImage.Source = RAMTool.Decolorize(bitmapImage);
            }
            else if (commond == "二值化")
            {
                //targetImage.Source = RAMTool.GTo2Bit(bitmapImage);
                targetImage.Source = PointTool.OtsuThreshold(bitmapImage);
            }
            else if (commond == "浮雕")
            {
                //targetImage.Source = ToolClass.Relief(bitmapImage);
            }
            else if (commond == "马赛克")
            {
                //targetImage.Source = ToolClass.Mosaic(bitmapImage);
            }
            else if (commond == "扩散")
            {
                //targetImage.Source = ToolClass.Biffusion(bitmapImage);
            }
        }

        private void HistTool_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmapImage = targetImage.Source as BitmapImage;
            Bitmap bitmap = HelpClass.BitmapImage2Bitmap(bitmapImage);

            MenuItem mi = e.Source as MenuItem;
            string commond = mi.Header.ToString();
            if (commond == "灰度直方图")
            {
                Hist_Form hist_Form = new Hist_Form(bitmap);
                hist_Form.Show();
            }
        }
    }
}
