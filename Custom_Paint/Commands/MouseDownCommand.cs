using Custom_Paint.Contract2;
using Custom_Paint.Helper;
using Custom_Paint.ViewModels;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Custom_Paint.Commands
{
    public class MouseDownCommand : CommandBase
    {

        private PaintViewModel _viewModel;
        public MouseDownCommand(PaintViewModel vm)
        {
            _viewModel = vm;
        }

        public override void Execute(object? parameter)
        {
            if (parameter != null)
            {
                // Fill
                if (_viewModel.FillMode == true)
                {
                    var point = (Point)parameter;
                    var mainCanvas = _viewModel.GetMainCanvasFunc.Invoke();
                    var ps = new PixelScanner(mainCanvas);
                    var s = ps.ColorScan(point);
                    var fill = new FillObject(s);
                    _viewModel.PreviewObject = fill;
                    _viewModel.AcceptPreview();
                }
                // Draw
                else if (_viewModel.ChoosenShape != null)
                {
                    // Insert previous / for edit-able objects
                    if (_viewModel.PreviewObject != null)
                    {
                        _viewModel.AcceptPreview();
                    }
                    // Create new
                    _viewModel.IsDrawing = true;
                    _viewModel.PreviewObject = _viewModel.Factory.CreateShape(_viewModel.ChoosenShape);
                    _viewModel.Start = (Point)parameter;
                    _viewModel.PreviewObject.points = new List<Point>() { _viewModel.Start, _viewModel.Start };
                    _viewModel.PreviewObject.StrokeThickness = _viewModel.CurrentStrokeThickness;
                    _viewModel.PreviewObject.StrokeColor = _viewModel.CurrentColor;
                    _viewModel.PreviewObject.Fill = Brushes.Transparent;
                }
            }
        }
    }
}
