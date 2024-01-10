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
                    // Insert previous
                    if (_viewModel.Preview != null)
                    {
                        _viewModel.Preview.HideAdorner();
                        _viewModel.AcceptReview.Invoke(_viewModel.Preview.Draw());
                        _viewModel.Preview = null;
                    }
                    // Create new
                    
                    {
                        _viewModel.Preview = _viewModel.Factory.CreateShape(_viewModel.ChoosenShape);
                        _viewModel.IsDrawing = true;
                        _viewModel.Start = (Point)parameter;
                        _viewModel.Preview.points = new List<Point>() { _viewModel.Start, _viewModel.Start };
                        _viewModel.Preview.StrokeThickness = _viewModel.CurrentStrokeThickness;
                        _viewModel.Preview.StrokeColor = _viewModel.CurrentColor;
                        _viewModel.Preview.Fill = Brushes.Transparent;
                    }
                }
            }
        }
    }
}
