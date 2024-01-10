using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<Point>? points { get; set; }
        public double StrokeThickness { get; set; }
        public double Angle { get; set; } = 0;
        public double centerX { get; set; }
        public double centerY { get; set; }
        public SolidColorBrush? Fill { get; set; } = null;
        public SolidColorBrush? StrokeColor { get; set; } = null;

        public string SaveText()
        {
            var b = Fill.Color.R; var c= Fill.Color.G; var d = Fill.Color.B; var e = Fill.Color.A;
        }
    }


    public abstract class IShape
    {
        public abstract string Name { get; }
        public abstract string Icon { get; }

        public List<Point>? points { get; set; }
        public double StrokeThickness { get; set; }
        public double Angle { get; set; } = 0;
        public double centerX { get; set; }
        public double centerY { get; set; }
        public SolidColorBrush? Fill { get; set; } = null;
        public SolidColorBrush? StrokeColor { get; set; } = null;

        public UIElement? Preview { get; set; }
        public bool isSelected = false;

        public abstract void UpdatePoints(Point newPoint);
        public abstract IShape Clone();
        public abstract UIElement Draw();

        public abstract void HideAdorner();
        public abstract void ShowAdorner();

        protected ShapeInfo LoadInfo(string dir_name, IReader reader)
        {
            return reader.Read(dir_name);
        }
        public abstract IShape LoadShape(string dir_name, IReader reader);
    }
}
