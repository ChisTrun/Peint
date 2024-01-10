using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public interface IWriter
    {
        void Write(string dir_folder, List<IShape> Storage);
    }
}
