using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom_Paint.Helper
{
    public class FileWriter
    {
        public IWriter Writer { get; set; }

        public FileWriter(IWriter writer)
        {
            Writer = writer;
        }
    }
}
