using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public class ReaderJSON : IReader
    {
        public List<ShapeInfo> Read(string dir_name)
        {
            var json = File.ReadAllText(dir_name);
            var serializableList = JsonConvert.DeserializeObject<List<SerializableShapeInfo>>(json);
            return serializableList.ConvertAll(item => item.ToShapeInfo());
        }
    }
}
