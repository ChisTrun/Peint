using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Contract
{
    public class WriterBinary : IWriter
    {
        public void Write(string dir_folder, List<IShape> Storage)
        {
            string fileName = Path.Combine(dir_folder, "ExportBinary.bin");
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
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

                    List<object> objects = info.GetInfo();

                    foreach (object obj in objects)
                    {
                        if (obj is int)
                        {
                            writer.Write((int)obj);
                        }
                        else if (obj is double)
                        {
                            writer.Write((double)obj);
                        }
                        else if (obj is string)
                        {
                            writer.Write((string)obj);
                        }
                        else if (obj is byte)
                        {
                            writer.Write((byte)obj);
                        }
                    }
                }
            }
        }
    }
}
