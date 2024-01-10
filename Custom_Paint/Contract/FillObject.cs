using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Custom_Paint.Contract2
{
    public class FillObject : IShape
    {
        public FillObject()
        {
            this.Preview = new UIElement();
        }
        public FillObject(UIElement? element)
        {
            this.Preview = element;
        }

        public override string Name => "fill";

        public override string Icon => "🧺";

        public override ObjType ObjType => ObjType.Outside;

        public override IShape Clone()
        {
            return new FillObject(this.Preview);
        }

        public override UIElement Draw()
        {
            return this.Preview ?? new UIElement();
        }

        public override void HideAdorner()
        {
        }

        public override void ShowAdorner()
        {
        }

        public override void UpdatePoints(Point newPoint)
        {
        }
    }
}
