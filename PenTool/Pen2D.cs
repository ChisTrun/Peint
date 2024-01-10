
using Contract;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PenTool
{
    public class Pen2D : IShape
    {
        public override string Name => "pen";

        public override string Icon => "🖋️";

        public override IShape Clone()
        {
            return new Pen2D();
        }

        public override UIElement Draw()
        {
            GeometryCollection Children = new GeometryCollection();
            for (int i = 0; i < points.Count - 1; i++)
            {
                Children.Add(new LineGeometry(points[i], points[i + 1]));
            }
            GeometryGroup Data = new GeometryGroup() { Children = Children };
            Path path = new Path()
            {
                Data = Data,
                StrokeThickness = this.StrokeThickness,
                Stroke = this.StrokeColor
            };
            this.Preview = path;
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
            if (points != null)
                points.Add(newPoint);
        }
        // load
    }

}
