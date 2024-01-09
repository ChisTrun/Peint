using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Adorner
{
    public class RectRezize : Adorner
    {
        VisualCollection AdornerVisuals;
        Thumb thumb1;
        Thumb thumb2;
        Rectangle thumbRect;
        public ResizeAdorner(UIElement adornedElement) : base(adornedElement)
        {
            AdornerVisuals = new VisualCollection(this);
            thumb1 = new Thumb() { Background = Brushes.Orange, Height = 10, Width = 10 };
            thumb2 = new Thumb() { Background = Brushes.Orange, Height = 10, Width = 10 };
            thumbRect = new Rectangle() { Stroke = Brushes.Orange, StrokeThickness = 2, StrokeDashArray = { 3, 2 } };

            thumb1.DragDelta += DragDelta_Thumb1;
            thumb2.DragDelta += DragDelta_Thumb2;

            AdornerVisuals.Add(thumbRect);
            AdornerVisuals.Add(thumb1);
            AdornerVisuals.Add(thumb2);
        }

        protected override Visual GetVisualChild(int index)
        {
            return AdornerVisuals[index];
        }

        protected override int VisualChildrenCount => AdornerVisuals.Count;

        protected override Size ArrangeOverride(Size finalSize)
        {

            thumb1.Arrange(new Rect(-10, -10, 10, 10));
            thumb2.Arrange(new Rect(AdornedElement.DesiredSize.Width, AdornedElement.DesiredSize.Height, 10, 10));
            thumbRect.Arrange(new Rect(-5, -5, AdornedElement.DesiredSize.Width + 10, AdornedElement.DesiredSize.Height + 10));
            return base.ArrangeOverride(finalSize);
        }

        private void DragDelta_Thumb2(object sender, DragDeltaEventArgs e)
        {
            var ele = (FrameworkElement)AdornedElement;
            ele.Height = ele.Height + e.VerticalChange < 0 ? 0 : ele.Height + e.VerticalChange;
            ele.Width = ele.Width + e.HorizontalChange < 0 ? 0 : ele.Width + e.HorizontalChange;
        }

        private void DragDelta_Thumb1(object sender, DragDeltaEventArgs e)
        {
            var ele = (FrameworkElement)AdornedElement;
            var deltaTop = e.VerticalChange;
            var deltaLeft = e.HorizontalChange;
            var currentTop = Canvas.GetTop(ele) + ele.Height;
            var currentLeft = Canvas.GetLeft(ele) + ele.Width;
            if (Canvas.GetTop(ele) + e.VerticalChange > currentTop)
            {
                deltaTop = 0;
            }
            if (Canvas.GetLeft(ele) + e.HorizontalChange > currentLeft)
            {
                deltaLeft = 0;
            }

            Canvas.SetTop(ele, deltaTop + Canvas.GetTop(ele));
            Canvas.SetLeft(ele, deltaLeft + Canvas.GetLeft(ele));
            ele.Height = ele.Height - deltaTop < 0 ? 0 : ele.Height - deltaTop;
            ele.Width = ele.Width - deltaLeft < 0 ? 0 : ele.Width - deltaLeft;

        }
    }

}
