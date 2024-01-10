using Contract;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Custom_Paint.Services
{
    public class ReaderBinary : IReader
    {
        List<ShapeInfo> IReader.Read(string dir_name)
        {
            List<ShapeInfo> elements = new List<ShapeInfo>();

            using (BinaryReader reader = new BinaryReader(File.Open(dir_name, FileMode.Open)))
            {
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    string nName = reader.ReadString();
                    int npointsCount = reader.ReadInt32();

                    List<Point> npoints = new List<Point>();
                    for (int i = 0; i < npointsCount; i++)
                    {
                        Point p = new Point();

                        p.X = reader.ReadDouble();
                        p.Y = reader.ReadDouble();

                        npoints.Add(p);
                    }

                    double nStrokeThickness = reader.ReadDouble();
                    double nAngle = reader.ReadDouble();
                    byte nr = reader.ReadByte();
                    byte ng = reader.ReadByte();
                    byte nb = reader.ReadByte();
                    byte na = reader.ReadByte();
                    SolidColorBrush nStrokeColor = new SolidColorBrush() { Color = Color.FromArgb(na, nr, ng, nb) };

                    ShapeInfo newShapeInfo = new ShapeInfo()
                    {
                        Name = nName,
                        points = npoints,
                        StrokeThickness = nStrokeThickness,
                        Angle = nAngle,
                        StrokeColor = nStrokeColor
                    };

                    elements.Add(newShapeInfo);
                }
            }

            return elements;

        }
    }
}
