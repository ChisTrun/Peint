using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom_Paint.Services
{
    public class ShapeFactory
    {
        public Dictionary<string, IShape> prototypes = new Dictionary<string, IShape>();

        public IShape CreateShape(string choose)
        {
            return prototypes[choose].Clone();
        }

    }
}
