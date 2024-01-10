using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml.Serialization;

namespace Contract
{
    public class WriterXML : IWriter
    {
        public void Write(string dir_folder, List<IShape> Storage)
        {
            List<ShapeInfo> listShapeInfo = new List<ShapeInfo>();
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

                listShapeInfo.Add(info);
            }

            string fileName = Path.Combine(dir_folder, "ExportXML.xml");
            var serializableList = listShapeInfo.ConvertAll(SerializableShapeInfo.FromShapeInfo);
            var serializer = new XmlSerializer(typeof(List<SerializableShapeInfo>));
            using (var writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, serializableList);
            }
        }
    }
}
