﻿using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AdornerLib
{
    public class LineResize : Adorner
    {
        VisualCollection AdornerVisuals;
        Thumb TopLeft;
        Thumb TopRight;
        Thumb BotRight;
        Thumb Center;
        Thumb Rotate;
        Thumb BotLeft;

        Thumb Start;
        Thumb End;


        Rectangle thumbRect;
        IShape ishape;
        public LineResize(UIElement adornedElement, IShape ishape) : base(adornedElement)
        {
            this.ishape = ishape;
            AdornerVisuals = new VisualCollection(this);
            Start = new Thumb() { Background = Brushes.Orange, Height = 10, Width = 10 };
            End = new Thumb() { Background = Brushes.Orange, Height = 10, Width = 10 };

            Start.DragDelta += StartDragDelta;
            End.DragDelta += EndDragDelta;

            AdornerVisuals.Add(Start);
            AdornerVisuals.Add(End);
        }

        private void EndDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (ishape.points != null)
            {
                var deltaY = e.VerticalChange;
                var deltaX = e.HorizontalChange;
                var ele = (Line)AdornedElement;
                ele.X2 = deltaX + ishape.points[1].X;
                ele.Y2 = deltaY + ishape.points[1].Y;
                UpdatePointIshape(ishape.points[0], new Point(ele.X2, ele.Y2));
            }
        }

        private void StartDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (ishape.points != null)
            {
                var deltaY = e.VerticalChange;
                var deltaX = e.HorizontalChange;
                var ele = (Line)AdornedElement;
                ele.X1 = deltaX + ishape.points[0].X;
                ele.Y1 = deltaY + ishape.points[0].Y;
                UpdatePointIshape(new Point(ele.X1, ele.Y1), ishape.points[1]);
            }
        }

        protected void UpdatePointIshape(Point a, Point b)
        {
            if (this.ishape.points != null)
            {
                ishape.points[0] = a;
                ishape.points[1] = b;
            }
        }
        protected override Visual GetVisualChild(int index)
        {
            return AdornerVisuals[index];
        }

        protected override int VisualChildrenCount => AdornerVisuals.Count;

        protected override Size ArrangeOverride(Size finalSize)
        {
            Start.Arrange(new Rect(this.ishape.points[0].X - 10, this.ishape.points[0].Y - 10, 10, 10));
            End.Arrange(new Rect(this.ishape.points[1].X, this.ishape.points[1].Y, 10, 10));
            return base.ArrangeOverride(finalSize);
        }
    }
}
