using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom_Paint.Helper
{
    public class FileReader
    {
        public IReader Reader { get; set; }

        public FileReader(IReader reader)
        {
            this.Reader = reader;
        }
    }
}
