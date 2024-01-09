
using Contract;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LineShape
{
    public class Line2D : IShape
    {
        public override string Name => "Line";

        public override string Icon =>  "⁄";

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
            };
            return line;
        }

        public override void HideAdorner()
        {
            throw new NotImplementedException();
        }

        public override void ShowAdorner()
        {
            throw new NotImplementedException();
        }

        public override void UpdatePoints(Point newPoint)
        {
            this.points[1] = newPoint;
        }
    }

}
