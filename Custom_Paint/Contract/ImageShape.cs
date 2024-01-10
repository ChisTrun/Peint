using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Custom_Paint.Contract
{
    public class ImageShape : IShape
    {
        public ImageShape()
        {
            this.Preview = new UIElement();
        }
        public ImageShape(UIElement? element)
        {
            this.Preview = element;
        }

        public override ObjType ObjType => ObjType.Outside;

        public override string Name => "img";

        public override string Icon => "🖼️";

        public override IShape Clone()
        {
            return new ImageShape();
        }

        public override UIElement Draw()
        {
            return this.Preview;
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
