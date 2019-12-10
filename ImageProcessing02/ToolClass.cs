using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing.Imaging;

namespace ImageProcessing02
{
    public class ToolClass
    {
        //暗角
        public static BitmapImage Vignetting(BitmapImage bitmapImage)
        {
            Bitmap bitmap = BitmapImage2Bitmap(bitmapImage);

            int width = bitmap.Width;
            int height = bitmap.Height;
            double cx = width / 2;
            double cy = height / 2;
            double maxdist = cx * cx + cy * cy;
            double currdist = 0, factor;
            Color pixel;
            for(int i=0; i<width; i++)
            {
                for(int j=0; j<height; j++)
                {
                    currdist = (i - cx) * (i - cx) + (j - cy) * (j - cy);
                    factor = currdist / maxdist;
                    pixel = bitmap.GetPixel(i,j);
                    int red = (int)(pixel.R * (1 - factor));
                    int green = (int)(pixel.G * (1 - factor));
                    int blue = (int)(pixel.B * (1 - factor));

                    bitmap.SetPixel(i, j, Color.FromArgb(red,green,blue));
                }
            }

            return Bitmap2BitmapImage(bitmap);
        }

        //亮度
        public static BitmapImage Brightness(BitmapImage bitmapImage)
        {
            Bitmap bitmap = BitmapImage2Bitmap(bitmapImage);
            int red, green, blue;
            Color pixel;
            for(int i = 0; i < bitmap.Width; i++)
            {
                for(int j = 0; j < bitmap.Height; j++)
                {
                    pixel = bitmap.GetPixel(i, j);
                    red = (int)(pixel.R * 0.6);
                    green = (int)(pixel.G * 0.6);
                    blue = (int)(pixel.B * 0.6);
                    bitmap.SetPixel(i, j, Color.FromArgb(red, green, blue));
                }
            }

            return Bitmap2BitmapImage(bitmap);
        }

        //去色
        //gray = 0.3 * R + 0.59 * G + 0.11 * B
        public static BitmapImage Decolorize(BitmapImage bitmapImage)
        {
            Bitmap bitmap = BitmapImage2Bitmap(bitmapImage);
            int gray;
            Color pixel;
            for(int i = 0; i < bitmap.Width; i++)
            {
                for(int j=0; j < bitmap.Height; j++)
                {
                    pixel = bitmap.GetPixel(i, j);
                    gray = (int)(0.3 * pixel.R + 0.59 * pixel.G + 0.11 * pixel.B);
                    bitmap.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
            return Bitmap2BitmapImage(bitmap);
        }

        //浮雕
        public static BitmapImage Relief(BitmapImage bitmapImage)
        {
            Bitmap bitmap = BitmapImage2Bitmap(bitmapImage);
            int red, green, blue;
            Color pixel;
            for(int i = 0; i < bitmap.Width; i++)
            {
                for(int j=0; j < bitmap.Height; j++)
                {
                    pixel = bitmap.GetPixel(i, j);
                    red = (int)(255 - pixel.R);
                    green = (int)(255 - pixel.G);
                    blue = (int)(255 - pixel.B);
                    bitmap.SetPixel(i, j, Color.FromArgb(red, green, blue));
                }
            }
            return Bitmap2BitmapImage(bitmap);
        }

        //马赛克
        public static BitmapImage Mosaic(BitmapImage bitmapImage)
        {
            Bitmap bitmap = BitmapImage2Bitmap(bitmapImage);
            int RIDIO = 50;
            for(int h=0; h < bitmap.Height; h += RIDIO)
            {
                for(int w = 0; w < bitmap.Width; w += RIDIO)
                {
                    int avgRed = 0, avgGreen = 0, avgBlue = 0;
                    int count = 0;
                    for(int x = w; (x < w + RIDIO && x < bitmap.Width); x++)
                    {
                        for(int y = h; (y < h + RIDIO && y < bitmap.Height); y++)
                        {
                            Color pixel = bitmap.GetPixel(x, y);
                            avgRed += pixel.R;
                            avgGreen += pixel.G;
                            avgBlue += pixel.B;
                            count++;
                        }
                    }

                    avgRed = avgRed / count;
                    avgGreen = avgGreen / count;
                    avgBlue = avgBlue / count;

                    for(int x = w; (x<w+RIDIO && x < bitmap.Width); x++)
                    {
                        for(int y = h; (y < h + RIDIO && y < bitmap.Height); y++)
                        {
                            bitmap.SetPixel(x, y, Color.FromArgb(avgRed, avgGreen, avgBlue));
                        }
                    }
                }
            }
            return Bitmap2BitmapImage(bitmap);
        }

        //扩散
        public static BitmapImage Biffusion(BitmapImage bitmapImae)
        {
            Bitmap bitmap = BitmapImage2Bitmap(bitmapImae);
            int red, green, blue;
            int flag = 0;
            Color pixel;
            for(int i = 0; i < bitmap.Width; i++)
            {
                for(int j = 0; j < bitmap.Height; j++)
                {
                    Random random = new Random();
                    int RankKey = random.Next(-5, 5);
                    if(i + RankKey >= bitmap.Width || j + RankKey >= bitmap.Height || i + RankKey < 0 || j + RankKey < 0)
                    {
                        flag = 1;
                        continue;
                    }

                    pixel = bitmap.GetPixel(i + RankKey, j + RankKey);
                    red = (int)(pixel.R);
                    green = (int)(pixel.G);
                    blue = (int)(pixel.B);
                    bitmap.SetPixel(i, j, Color.FromArgb(red, green, blue));
                }
            }
            return Bitmap2BitmapImage(bitmap);
        }

        private static Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using(MemoryStream stream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(stream);
                Bitmap bitmap = new Bitmap(stream);

                return new Bitmap(bitmap);
            }
        }

        private static BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            using(MemoryStream stream = new MemoryStream())
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
    }
}
