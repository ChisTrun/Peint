using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Contract
{
    public class ReaderText : IReader
    {
        public List<ShapeInfo> Read(string dir_name)
        {
            List<ShapeInfo> list = new List<ShapeInfo>();

            using (StreamReader reader = new StreamReader(dir_name))
            {
                ShapeInfo newShapeInfo = new ShapeInfo();
                string nName = "";
                int npointCount = 0;
                List<Point> npoints = new List<Point>();
                double nStrokeThickness = 0;
                double nAngle = 0;
                SolidColorBrush nStrokeColor = new SolidColorBrush();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');

                    if (parts[0] == "Name")
                    {
                        nName = parts[1];
                    }
                    else if (parts[0] == "PointCount")
                    {
                        npointCount = int.Parse(parts[1]);
                    }
                    else if (parts[0] == "Point")
                    {
                        npoints.Add(new Point(double.Parse(parts[1]), double.Parse(parts[2])));
                    }
                    else if (parts[0] == "StrokeThickness")
                    {
                        nStrokeThickness = double.Parse(parts[1]);
                    }
                    else if (parts[0] == "Angle")
                    {
                        nAngle = double.Parse(parts[1]);
                    }
                    else if (parts[0] == "StrokeColor")
                    {
                        nStrokeColor.Color = Color.FromArgb(byte.Parse(parts[1]), byte.Parse(parts[2]), byte.Parse(parts[3]), byte.Parse(parts[4]));

                        newShapeInfo.Name = nName;
                        newShapeInfo.points = npoints;
                        newShapeInfo.StrokeThickness = nStrokeThickness;
                        newShapeInfo.Angle = nAngle;
                        newShapeInfo.StrokeColor = nStrokeColor;

                        list.Add(newShapeInfo);

                        newShapeInfo = new ShapeInfo();
                        nName = "";
                        npointCount = 0;
                        npoints = new List<Point>();
                        nStrokeThickness = 0;
                        nAngle = 0;
                        nStrokeColor = new SolidColorBrush();
                    }
                }
            }

            return list;
        }
    }
}
