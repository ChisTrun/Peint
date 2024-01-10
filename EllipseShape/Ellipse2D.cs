
using AdornerLib;
using Contract;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EllipseShape
{
    public class Ellipse2D : IShape
    {
        public override string Name => "Ellipse";

        public override string Icon => "○";

        public override Contract.Type ObjType => Contract.Type.Shape;

        public override IShape Clone()
        {
            return new Ellipse2D();
        }

        public override UIElement Draw()
        {
            var deltaX = 0.0;
            var deltaY = 0.0;
            var _width = points[1].X - points[0].X;
            var _height = points[1].Y - points[0].Y;
            if (_width < 0) _width = 0;
            if (_height < 0) _height = 0;

            this.centerX = _width / 2;
            this.centerY = _height / 2;

            Ellipse ellipse = new Ellipse()
            {
                Width = Math.Abs(_width),
                Height = Math.Abs(_height),
                Stroke = this.StrokeColor,
                StrokeThickness = this.StrokeThickness,
                Fill = this.Fill,
                RenderTransform = new TransformGroup()
                {
                    Children = new TransformCollection()
                    {
                        new RotateTransform(this.Angle, this.centerX, this.centerY),
                        new ScaleTransform(this.FlipV, this.FlipH, this.centerX, this.centerY)
                    }
                },
                StrokeDashArray = this.StrokeDashArray,
            };
            Canvas.SetTop(ellipse, points[0].Y);
            Canvas.SetLeft(ellipse, points[0].X );
            this.Preview = ellipse;
            return this.Preview;
        }

        public override void UpdatePoints(Point newPoint)
        {
            if (this.points != null) this.points[1] = newPoint;
        }

        public override void ShowAdorner()
        {
            if (this.Preview != null)
            {
                this.isSelected = true;
                AdornerLayer.GetAdornerLayer(VisualTreeHelper.GetParent(this.Preview) as UIElement).Add(new RectResize(this.Preview, this));
            }
            
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
    }

}
