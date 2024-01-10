using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace Custom_Paint.Helper
{
    public class PreviewUpdater
    {
        public static void Test(Canvas DrawCanvas, Image DrawImage)
        {
            try
            {
                Rect bounds = VisualTreeHelper.GetDescendantBounds(DrawCanvas);
                double dpi = 96d;
                RenderTargetBitmap rtb = new RenderTargetBitmap(
                    (int)DrawCanvas.ActualWidth,
                    (int)DrawCanvas.ActualHeight,
                    dpi,
                    dpi,
                    PixelFormats.Pbgra32);
                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext dc = dv.RenderOpen())
                {
                    foreach (UIElement child in DrawCanvas.Children)
                    {
                        VisualBrush vb = new VisualBrush(child);
                        Rect childBounds = VisualTreeHelper.GetDescendantBounds(child as Visual);
                        var test = child.TransformToAncestor(DrawCanvas);
                        childBounds = test.TransformBounds(childBounds);
                        dc.DrawRectangle(vb, null, childBounds);
                    }
                }
                rtb.Render(dv);

                BitmapSource existingImage = (BitmapSource)DrawImage.Source;

                DrawingVisual overlayVisual = new DrawingVisual();
                using (DrawingContext dc = overlayVisual.RenderOpen())
                {
                    dc.DrawImage(existingImage, new Rect(0, 0, DrawImage.Width, DrawImage.Height));

                    dc.DrawImage(rtb, new Rect(0, 0, DrawImage.Width, DrawImage.Height));
                }

                RenderTargetBitmap overlayBitmap = new RenderTargetBitmap(
                    (int)DrawImage.Width,
                    (int)DrawImage.Height,
                    dpi,
                    dpi,
                    PixelFormats.Pbgra32);
                overlayBitmap.Render(overlayVisual);

                BitmapImage bitmapImage = new BitmapImage();
                using (MemoryStream memory = new MemoryStream())
                {
                    PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
                    pngEncoder.Frames.Add(BitmapFrame.Create(overlayBitmap));
                    pngEncoder.Save(memory);
                    memory.Position = 0;
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                }
                DrawImage.Source = bitmapImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public static void Update(Canvas StoreCanvas, Image DrawImage)
        {
            try
            {
                Rect bounds = VisualTreeHelper.GetDescendantBounds(StoreCanvas);
                double dpi = 96d;
                RenderTargetBitmap rtb = new RenderTargetBitmap(
                    (int)StoreCanvas.ActualWidth,
                    (int)StoreCanvas.ActualHeight,
                    dpi,
                    dpi,
                    PixelFormats.Pbgra32);
                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext dc = dv.RenderOpen())
                {
                    foreach (UIElement child in StoreCanvas.Children)
                    {
                        VisualBrush vb = new VisualBrush(child);
                        Rect childBounds = VisualTreeHelper.GetDescendantBounds(child as Visual);
                        childBounds = child.TransformToAncestor(StoreCanvas).TransformBounds(childBounds);
                        dc.DrawRectangle(vb, null, childBounds);
                    }
                }
                rtb.Render(dv);

                BitmapImage bitmapImage = new BitmapImage();
                using (MemoryStream memory = new MemoryStream())
                {
                    PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
                    pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
                    pngEncoder.Save(memory);
                    memory.Position = 0;
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                }
                DrawImage.Source = bitmapImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
