using System.Drawing;
using Image = System.Windows.Controls.Image;
using Point = System.Windows.Point;
using Color = System.Windows.Media.Color;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Runtime.InteropServices;
using Size = System.Windows.Size;
using System.IO;
using System.Windows.Media.Media3D;
using static System.Windows.Forms.DataFormats;
using System.Windows.Input;

namespace Custom_Paint.Helper
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PixelColor
    {
        public byte Blue;
        public byte Green;
        public byte Red;
        public byte Alpha;
        public Color Color => new Color() { R = Red, B = Blue, G = Green, A = Alpha };
    }

    public class PixelScanner
    {
        private readonly PixelColor[,] _pixelMap;

        public PixelScanner(Image image)
        {
            var source = image.Source as BitmapSource ?? throw new ArgumentException();
            _pixelMap = GetPixels(source);
        }

        private int Width => _pixelMap.GetLength(0);
        private int Height => _pixelMap.GetLength(1);
        private PixelColor GetPixelColorFromMap(Point point) => _pixelMap[(int)point.X, (int)point.Y];
        private bool IsInBound(Point point) => point.X >= 0 && point.X < Width && point.Y >= 0 && point.Y < Height;

        public UIElement ColorScan(Point start)
        {
            var baseColor = GetPixelColorFromMap(start).Color;

            // Scanning
            Vector goTop = new(0, -1);
            Vector goLeft = new(-1, 0);
            Vector goRight = new(1, 0);
            Vector goBottom = new(0, 1);

            var hasRead = new bool[Width, Height];
            bool GetHasRead(Point cur) => hasRead[(int)cur.X, (int)cur.Y];
            void SetHasRead(Point cur) => hasRead![(int)cur.X, (int)cur.Y] = true;

            var queue = new Queue<Point>();
            queue.Enqueue(start);
            while (queue.Count != 0)
            {
                var cur = queue.Dequeue();
                if (!IsInBound(cur) || GetHasRead(cur))
                {
                    continue;
                }
                var color = GetPixelColorFromMap(cur).Color;
                if (SimilarColor(color, baseColor) != true)
                {
                    continue;
                }
                SetHasRead(cur);
                var curTop = cur + goTop;
                var curLeft = cur + goLeft;
                var curRight = cur + goRight;
                var curBottom = cur + goBottom;
                queue.Enqueue(curTop);
                queue.Enqueue(curLeft);
                queue.Enqueue(curRight);
                queue.Enqueue(curBottom);
            }

            PixelColor[,] pixels = new PixelColor[Width, Height];
            for (int k = 0; k < hasRead.GetLength(0); k++)
            {
                for (int l = 0; l < hasRead.GetLength(1); l++)
                {
                    var val = hasRead[k, l];
                    if (val == true)
                    {
                        pixels[k, l] = new PixelColor() { Red = 100, Blue = 100, Green = 100, Alpha = 100, };
                    }
                    else
                    {
                        pixels[k, l] = new PixelColor() { Red = 100, Blue = 100, Green = 100, Alpha = 100, };
                    }
                }
            }
            WriteableBitmap bitmap = new WriteableBitmap(Width, Height, 96, 96, PixelFormats.Bgra32, null);
            //PixelColor[,] pixels = new PixelColor[Width, Height];
            //WriteableBitmap bitmap = new WriteableBitmap(Width, Height, 96, 96, PixelFormats.Bgra32, null);
            //for (int k = 0; k < Width; k++)
            //{
            //    for (int l = 0; l < Height; l++)
            //    {
            //        var val = hasRead[k, l];
            //        if (val == true)
            //        {
            //            var col = new PixelColor() { Red = 100, Blue = 100, Green = 100, Alpha = 100, };
            //            _pixelMap[k, l] = col;
            //            PutPixels(bitmap, _pixelMap, k, l);
            //        }
            //        else
            //        {
            //            var col = new PixelColor() { Red = 100, Blue = 100, Green = 100, Alpha = 100 };
            //            _pixelMap[k, l] = col;
            //            PutPixels(bitmap, _pixelMap, k, l);
            //        }
            //    }
            //}
            //bitmap.WritePixels(new Int32Rect(0, 0, Width, Height), pixels, Width * 4, 0, 0);
            Image a = new Image();
            a.Source = bitmap;
            return a;
        }

        public static bool SimilarColor(Color a, Color b, int diff = 0)
        {
            var diff_R = Math.Abs(a.R - b.R) <= diff;
            var diff_G = Math.Abs(a.G - b.G) <= diff;
            var diff_B = Math.Abs(a.B - b.B) <= diff;
            var diff_A = Math.Abs(a.A - b.A) <= diff;
            return diff_R && diff_G && diff_B && diff_A;
        }

        // Height, Width and y, x
        public static PixelColor[,] GetPixels(BitmapSource source)
        {
            if (source.Format != PixelFormats.Bgra32)
                source = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0);

            int width = source.PixelWidth; // | * 4
            int height = source.PixelHeight;
            PixelColor[,] result = new PixelColor[width, height];
            CopyPixels(source, result);
            return result;
        }

        public static void PutPixels(WriteableBitmap bitmap, PixelColor[,] pixels, int x, int y)
        {
            int width = pixels.GetLength(0);
            int height = pixels.GetLength(1);
            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, x, y);
        }

        public static void CopyPixels(BitmapSource source, PixelColor[,] pixels)
        {
            var offset = 0;
            var height = source.PixelHeight;
            var width = source.PixelWidth;

            int stride = (int)source.PixelWidth * 4;
            byte[] pixelBytes = new byte[height * stride];

            source.CopyPixels(pixelBytes, stride, offset);

            var test = pixelBytes.Where(x => x > 0).ToArray();

            int y0 = offset / width;
            int x0 = offset - width * y0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    pixels[x + x0, y + y0] = new PixelColor
                    {
                        Blue = pixelBytes[(y * width + x) * 4 + 0],
                        Green = pixelBytes[(y * width + x) * 4 + 1],
                        Red = pixelBytes[(y * width + x) * 4 + 2],
                        Alpha = pixelBytes[(y * width + x) * 4 + 3],
                    };
                }
            }
        }



        //public static Image PixelFill(double x, double y, Image source)
        //{
        //    WriteableBitmap writeableBitmap = new WriteableBitmap(source.Source as BitmapSource);
        //    int column = (int)x;
        //    int row = (int)y;

        //    try
        //    {
        //        // Reserve the back buffer for updates.
        //        writeableBitmap.Lock();
        //        unsafe
        //        {
        //            // Get a pointer to the back buffer.
        //            IntPtr pBackBuffer = writeableBitmap.BackBuffer;

        //            // Find the address of the pixel to draw.
        //            pBackBuffer += row * writeableBitmap.BackBufferStride;
        //            pBackBuffer += column * 4;

        //            // Compute the pixel's color.
        //            int color_data = 255 << 16; // R
        //            color_data |= 128 << 8;   // G
        //            color_data |= 255 << 0;   // B

        //            // Assign the color data to the pixel.
        //            *((int*)pBackBuffer) = color_data;
        //        }

        //        // Specify the area of the bitmap that changed.
        //        writeableBitmap.AddDirtyRect(new Int32Rect(column, row, 1, 1));
        //    }
        //    finally
        //    {
        //        // Release the back buffer and make it available for display.
        //        writeableBitmap.Unlock();
        //    }
        //    return new Image() { Source = writeableBitmap };
        //}
    }
}
