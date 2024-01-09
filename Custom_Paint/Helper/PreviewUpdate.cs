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
    public class PreviewUpdate
    {
        public static void PreviewUpdater(Canvas PreviewCanvas, Image DrawImage)
        {
            try
            {
                Rect bounds = VisualTreeHelper.GetDescendantBounds(PreviewCanvas);
                double dpi = 96d;
                RenderTargetBitmap rtb = new RenderTargetBitmap(
                    (int)PreviewCanvas.ActualWidth,
                    (int)PreviewCanvas.ActualHeight,
                    dpi,
                    dpi,
                    PixelFormats.Pbgra32);
                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext dc = dv.RenderOpen())
                {
                    foreach (UIElement child in PreviewCanvas.Children)
                    {
                        VisualBrush vb = new VisualBrush(child);
                        Rect childBounds = VisualTreeHelper.GetDescendantBounds(child as Visual);
                        childBounds = child.TransformToAncestor(PreviewCanvas).TransformBounds(childBounds);
                        dc.DrawRectangle(vb, null, childBounds);
                    }
                }
                rtb.Render(dv);

                //BitmapSource existingImage = (BitmapSource)DrawImage.Source;

                //DrawingVisual overlayVisual = new DrawingVisual();
                //using (DrawingContext dc = overlayVisual.RenderOpen())
                //{
                //    dc.DrawImage(existingImage, new Rect(0, 0, DrawImage.Width, DrawImage.Height));
                //    dc.DrawImage(rtb, new Rect(0, 0, DrawImage.Width, DrawImage.Height));
                //}

                //RenderTargetBitmap overlayBitmap = new RenderTargetBitmap(
                //    (int)DrawImage.Width,
                //    (int)DrawImage.Height,
                //    dpi,
                //    dpi,
                //    PixelFormats.Pbgra32);
                //overlayBitmap.Render(overlayVisual);

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
