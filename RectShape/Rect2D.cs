
using Contract;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Documents;
using AdornerLib;

namespace RectShape
{
    public class Rect2D : IShape
    {
        public override string Name => "rect";

        public override string Icon => "▬";

        public override IShape Clone()
        {
            return new Rect2D();
        }

        public override UIElement Draw()
        {
            var deltaX = 0.0;
            var deltaY = 0.0;
            var _width = points[1].X - points[0].X;
            var _height = points[1].Y - points[0].Y;
            if (_width < 0)
                deltaX = _width;
            if (_height < 0)
                deltaY = _height;
            this.centerX = _width / 2;
            this.centerY = _height / 2;

            Rectangle rect = new Rectangle()
            {
                Width = Math.Abs(_width),
                Height = Math.Abs(_height),
                Stroke = this.StrokeColor,
                StrokeThickness = this.StrokeThickness,
                Fill = this.Fill,
                RenderTransform = new RotateTransform(this.Angle, this.centerX, this.centerY)
            };
            Canvas.SetTop(rect, points[0].Y + deltaY);
            Canvas.SetLeft(rect, points[0].X + deltaX);
            this.Preview = rect;
            return Preview;
        }

        public override void UpdatePoints(Point newPoint)
        {
            if (this.points != null)
                this.points[1] = newPoint;
        }

        public override void HideAdorner()
        {
            if (this.Preview != null)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this.Preview);
                Adorner[] adorners = adornerLayer.GetAdorners(this.Preview);
                if (adorners != null)
                {
                    foreach (Adorner adorner in adorners)
                    {
                        adornerLayer.Remove(adorner);
                    }
                }
                this.isSelected = false;
            }

        }
        public override void ShowAdorner()
        {
            if (this.Preview != null)
            {
                this.isSelected = true;
                AdornerLayer.GetAdornerLayer(VisualTreeHelper.GetParent(this.Preview) as UIElement).Add(new RectResize(this.Preview, this));
            }
        }

        public override IShape LoadShape(string dir_name, IReader reader)
        {
            var info = this.LoadInfo(dir_name, reader);
            this.centerX = info.centerX;
            this.centerY = info.centerY;
            return this;
        }
    }
}
