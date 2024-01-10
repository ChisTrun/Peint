
using AdornerLib;
using Contract;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LineShape
{
    public class Line2D : IShape
    {
        public override string Name => "Line";

        public override string Icon =>  "⁄";

        public override Contract.Type ObjType => Contract.Type.Shape;

        public override IShape Clone()
        {
            return new Line2D();    
        }

        public override UIElement Draw()
        {
            Line line = new Line()
            {
                X1 = points[0].X,
                Y1 = points[0].Y,
                X2 = points[1].X,
                Y2 = points[1].Y,
                Stroke = Brushes.Black,
                StrokeThickness = this.StrokeThickness,
                StrokeDashArray = this.StrokeDashArray, 
            };
            this.Preview = line;    
            return this.Preview;
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
                AdornerLayer.GetAdornerLayer(VisualTreeHelper.GetParent(this.Preview) as UIElement).Add(new LineResize(this.Preview, this));
            }
        }

        public override void UpdatePoints(Point newPoint)
        {
            this.points[1] = newPoint;
        }
    }

}
