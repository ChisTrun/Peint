
using Contract;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EraserTool
{
    public class Eraser2D : IShape
    {
        public override Contract.ObjType ObjType => Contract.ObjType.Tool;

        public override string Name => "eraser";

        public override string Icon => "⌫";

        public override IShape Clone()
        {
            return new Eraser2D();
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
                Stroke = Brushes.White,
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
            if (points != null) points.Add(newPoint);
        }
    }

}
