﻿using System;
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
    public abstract class IShape
    {
        public UIElement? Preview { get; set; }

        public abstract void ShowAdorner();
    
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

        public abstract void HideAdorner();
        //public static void RemoveResize(UIElement shape) {
        //    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(shape);
        //    Adorner[] adorners = adornerLayer.GetAdorners(shape);
        //    if (adorners != null)
        //    {
        //        foreach (Adorner adorner in adorners)
        //        {
        //            adornerLayer.Remove(adorner);
        //        }
        //    }
        //}

    }
}
