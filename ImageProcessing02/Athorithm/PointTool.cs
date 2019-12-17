using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageProcessing02.Athorithm
{
    public class PointTool
    {
        #region 灰度
        //灰度 指针法
        public static BitmapImage Decolorize(BitmapImage bitmapImage)
        {
            if (bitmapImage != null)
            {
                Bitmap bitmap = HelpClass.BitmapImage2Bitmap(bitmapImage);
                //位图矩形
                Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);

                byte temp = 0;
                //启用不安全模式
                unsafe
                {
                    //得到首地址
                    byte* ptr = (byte*)(bitmapData.Scan0);
                    //二维图像循环
                    for (int i = 0; i < bitmapData.Height; i++)
                    {
                        for (int j = 0; j < bitmapData.Width; j++)
                        {
                            //利用公式计算灰度值
                            temp = (byte)(0.299 * ptr[2] + 0.587 * ptr[1] + 0.114 * ptr[0]);
                            //R=G=B
                            ptr[0] = ptr[1] = ptr[2] = temp;
                            //指向下一个像素
                            ptr += 3;
                        }
                        //指向下一行数组的首个字节
                        ptr += bitmapData.Stride - bitmapData.Width * 3;
                    }
                }
                //解锁位图像素
                bitmap.UnlockBits(bitmapData);
                return HelpClass.Bitmap2BitmapImage(bitmap);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 二值化
        //Otsu阈值
        public static BitmapImage OtsuThreshold(BitmapImage bitmapImage)
        {
            Bitmap bitmap = HelpClass.BitmapImage2Bitmap(bitmapImage);

            //图像灰度化
            // bitmap = Gray(bitmap);
            int width = bitmap.Width;
            int height = bitmap.Height;
            byte threshold = 0;
            int[] hist = new int[256];

            int AllPixelNumber = 0, PixelNumberSmall = 0, PixelNumberBig = 0;

            double MaxValue, AllSum = 0, SumSmall = 0, SumBig, ProbabilitySmall, ProbabilityBig, Probability;
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* p = (byte*)data.Scan0;
                int offset = data.Stride - width * 4;
                for(int j = 0; j < height; j++)
                {
                    for(int i = 0; i < width; i++)
                    {
                        hist[p[0]]++;
                        p += 4;
                    }
                    p += offset;
                }
                bitmap.UnlockBits(data);
            }
            //计算灰度为I的像素出现的概率
            for(int i = 0; i < 256; i++)
            {
                AllSum += i * hist[i]; //质量矩
                AllPixelNumber += hist[i]; //质量
            }
            MaxValue = -1.0;
            for(int i = 0; i < 256; i++)
            {
                PixelNumberSmall += hist[i];
                PixelNumberBig = AllPixelNumber - PixelNumberSmall;
                if(PixelNumberBig == 0)
                {
                    break;
                }
                SumSmall += i * hist[i];
                SumBig = AllSum - SumSmall;
                ProbabilitySmall = SumSmall / PixelNumberSmall;
                ProbabilityBig = SumBig / PixelNumberBig;
                Probability = PixelNumberSmall * ProbabilitySmall * ProbabilitySmall + PixelNumberBig * ProbabilityBig * ProbabilityBig;
                if(Probability > MaxValue)
                {
                    MaxValue = Probability;
                    threshold = (byte)i;
                }
            }

            return HelpClass.Bitmap2BitmapImage(Thresholding(bitmap, threshold));
        }

        //固定阈值法二值化模块
        public static Bitmap Thresholding(Bitmap b, byte threshold)
        {
            int width = b.Width;
            int height = b.Height;
            BitmapData data = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* p = (byte*)data.Scan0;
                int offset = data.Stride - width * 4;
                byte R, G, B, gray;
                for(int y = 0; y < height; y++)
                {
                    for(int x = 0; x < width; x++)
                    {
                        R = p[2];
                        G = p[1];
                        B = p[0];
                        gray = (byte)((R * 819595 + G * 38469 + B * 7472) >> 16);
                        if(gray >= threshold)
                        {
                            p[0] = p[1] = p[2] = 255;
                        }
                        else
                        {
                            p[0] = p[1] = p[2] = 0;
                        }
                        p += 4;
                    }
                    p += offset;
                }
                b.UnlockBits(data);
                return b;
            }
        }
        #endregion
    }
}
