using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Contract
{
    public class ShapeInfo
    {
        public string Name { get; set; }
        public List<Point>? points { get; set; }
        public double StrokeThickness { get; set; }
        public double Angle { get; set; } = 0;
        public SolidColorBrush? StrokeColor { get; set; } = null;

        public List<object> GetInfo()
        {
            byte r = StrokeColor.Color.R;
            byte g = StrokeColor.Color.G;
            byte b = StrokeColor.Color.B;
            byte a = StrokeColor.Color.A;

            List<object> info = new List<object>
            {
                Name,
                points.Count - 1
            };

            for (int i = 0; i < points.Count - 1; i++)
            {
                info.Add(points[i].X);
                info.Add(points[i].Y);
            }

            info.Add(StrokeThickness);
            info.Add(Angle);
            info.Add(r);
            info.Add(g);
            info.Add(b);
            info.Add(a);

            return info;
        }
    }

    public class SerializableShapeInfo
    {
        public string Name { get; set; }
        public List<Point>? points { get; set; }
        public double StrokeThickness { get; set; }
        public double Angle { get; set; } = 0;
        public string StrokeColorString { get; set; }

        public ShapeInfo ToShapeInfo()
        {
            return new ShapeInfo
            {
                Name = this.Name,
                points = this.points,
                StrokeThickness = this.StrokeThickness,
                Angle = this.Angle,
                StrokeColor = !string.IsNullOrEmpty(this.StrokeColorString) ? new SolidColorBrush((Color)ColorConverter.ConvertFromString(this.StrokeColorString)) : null
            };
        }

        public static SerializableShapeInfo FromShapeInfo(ShapeInfo shapeInfo)
        {
            return new SerializableShapeInfo
            {
                Name = shapeInfo.Name,
                points = shapeInfo.points,
                StrokeThickness = shapeInfo.StrokeThickness,
                Angle = shapeInfo.Angle,
                StrokeColorString = shapeInfo.StrokeColor?.Color.ToString()
            };
        }
    }

    public abstract class IShape
    {
        public UIElement? Preview { get; set; }
        public double FlipV { get; set; } = 1;
        public double FlipH { get; set; } = 1;
        public abstract ObjType ObjType { get; }

        public bool isSelected = false;
        public List<Point>? points { get; set; }
        public double StrokeThickness { get; set; }
        public double Angle { get; set; } = 0;
        public double centerX { get; set; }
        public double centerY { get; set; }
        public SolidColorBrush? Fill { get; set; } = null;
        public SolidColorBrush? StrokeColor { get; set; } = null;
        public abstract string Name { get; }
        public abstract string Icon { get; }
        public abstract void UpdatePoints(Point newPoint);
        public abstract IShape Clone();
        public abstract UIElement Draw();
        public DoubleCollection? StrokeDashArray { get; set; } = new DoubleCollection(new double[] { });
        public abstract void HideAdorner();
        public abstract void ShowAdorner();

        public void LoadInfo(ShapeInfo si)
        {
            this.points = si.points;
            this.StrokeThickness = si.StrokeThickness;
            this.Angle = si.Angle;
            this.StrokeColor = si.StrokeColor;
        }
    }
}
