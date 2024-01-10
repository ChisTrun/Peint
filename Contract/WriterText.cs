using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Contract
{
    public class WriterText : IWriter
    {
        public void Write(string dir_folder, List<IShape> Storage)
        {
            string fileName = Path.Combine(dir_folder, "ExportText.txt");
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (IShape shape in Storage)
                {
                    ShapeInfo info = new ShapeInfo()
                    {
                        Name = shape.Name,
                        points = shape.points,
                        StrokeThickness = shape.StrokeThickness,
                        Angle = shape.Angle,
                        StrokeColor = shape.StrokeColor
                    };

                    int pointsCount = info.points.Count - 1;

                    writer.WriteLine($"Name,{info.Name}");
                    writer.WriteLine($"PointCount,{pointsCount}");
                    for (int i = 0; i < pointsCount; i++)
                    {
                        writer.WriteLine($"Point,{info.points[i].X},{info.points[i].Y}");
                    }
                    writer.WriteLine($"StrokeThickness,{info.StrokeThickness}");
                    writer.WriteLine($"Angle,{info.Angle}");
                    writer.WriteLine($"StrokeColor,{info.StrokeColor.Color.A},{info.StrokeColor.Color.R},{info.StrokeColor.Color.G},{info.StrokeColor.Color.B}");
                }
            }
        }
    }
}
