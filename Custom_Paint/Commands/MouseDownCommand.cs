using Contract;
using Custom_Paint.ViewModels;
using RectShape;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
                if (_viewModel.ChoosenShape != null)
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
                    _viewModel.PreviewObject.StrokeThickness = 2;
                    _viewModel.PreviewObject.StrokeColor = _viewModel.CurrentColor;
                    _viewModel.PreviewObject.Fill = Brushes.Transparent;
                }
            }
        }
    }
}
