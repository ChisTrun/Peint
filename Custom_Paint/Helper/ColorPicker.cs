using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using Image = System.Windows.Controls.Image;

namespace Custom_Paint.Helper
{
    public class ColorPicker
    {
        public static Color? Pick(double x, double y, Image source)
        {
            var bitmapSource = source.Source as BitmapSource;
            if (bitmapSource != null)
            {
                byte[] pixels = new byte[4];
                CroppedBitmap cb = new CroppedBitmap(bitmapSource, new Int32Rect((int)x, (int)y, 1, 1));
                cb.CopyPixels(pixels, 4, 0);
                return Color.FromArgb(pixels[3], pixels[2], pixels[1], pixels[0]);
            }
            return null;
        }
    }
}
