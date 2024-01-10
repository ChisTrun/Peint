﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using Contract;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Shapes;

namespace Custom_Paint.Helper
{
    public class PNGHelper
    {
        // fileName là đường dẫn tới file ảnh, có thể lấy bằng OpenFileDialog
        public static Image InsertImage(string fileName)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage(new Uri(fileName, UriKind.Absolute));

                Image newImage = new Image
                {
                    Width = bitmap.Width,
                    Height = bitmap.Height,
                    Source = bitmap
                };

                return newImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null!;
            }
        }

        // filePath là đường dẫn tới folder muốn lưu, có thể lấy bằng FolderBrowserDialog
        // có thể truyền vô fileName
        public static bool SavePng(string filePath, Image DrawImage)
        {
            try
            {
                BitmapSource bitmapSource = (BitmapSource)DrawImage.Source;

                PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                // có thể truyền vô fileName thế vào đây
                string fileName = System.IO.Path.Combine(filePath, "imgabc.png");
                using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                {
                    pngEncoder.Save(fileStream);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}