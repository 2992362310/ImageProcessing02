using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        public MainWindow()
        {
            InitializeComponent();
        }

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

        private void FileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = e.Source as MenuItem;
            if(mi.Header.ToString() == "打开图片")
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "所有图像文件|*.bmp;*.pcx;*.png;*.jpg;*.gif";
                openFileDialog.Title = "打开图像文件";
                if ((bool)openFileDialog.ShowDialog())
                {
                    originImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                }
            }
            else
            {
                this.Close();
            }
        }

        private void ToolMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate() {
            //    this.progressBar.IsIndeterminate = true;
            //});

            //new Thread(() =>
            //{
            //    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send,
            //        new Action(() => {
            //            this.progressBar.IsIndeterminate = true;
            //        }));
            //}).Start();

            BitmapImage bitmapImage = originImage.Source as BitmapImage;

            MenuItem mi = e.Source as MenuItem;
            string commond = mi.Header.ToString();
            if (commond == "暗角")
            {                
                targetImage.Source = ToolClass.Vignetting(bitmapImage);
            }
            else if(commond == "亮度")
            {
                targetImage.Source = ToolClass.Brightness(bitmapImage);
            }
            else if (commond == "去色")
            {
                targetImage.Source = ToolClass.Decolorize(bitmapImage);
            }
            else if (commond == "浮雕")
            {
                targetImage.Source = ToolClass.Relief(bitmapImage);
            }
            else if (commond == "马赛克")
            {
                targetImage.Source = ToolClass.Mosaic(bitmapImage);
            }
            else if (commond == "扩散")
            {
                targetImage.Source = ToolClass.Biffusion(bitmapImage);
            }
        }
    }
}
