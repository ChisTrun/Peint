using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Contract
{
    public class ReaderXML : IReader
    {
        public List<ShapeInfo> Read(string dir_name)
        {
            var serializer = new XmlSerializer(typeof(List<SerializableShapeInfo>));
            using (var reader = new StreamReader(dir_name))
            {
                var serializableList = (List<SerializableShapeInfo>)serializer.Deserialize(reader);
                return serializableList.ConvertAll(item => item.ToShapeInfo());
            }
        }
    }
}
