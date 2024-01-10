using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public interface IReader
    {
        ShapeInfo Read(string dir_name);
    }
}
