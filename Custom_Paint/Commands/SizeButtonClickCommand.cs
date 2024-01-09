using Contract;
using Custom_Paint.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Custom_Paint.Commands
{
    public class SizeButtonClickCommand : CommandBase
    {
        private PaintViewModel _viewModel;

        public SizeButtonClickCommand(PaintViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            if (parameter != null)
            {
                _viewModel.CurrentStrokeThickness = (double)parameter;
                foreach (IShape shape in _viewModel.ShapeList)
                {
                    if (shape.isSelected)
                    {
                        shape.StrokeThickness = _viewModel.CurrentStrokeThickness;
                    }
                }
                _viewModel.RenderList.Clear();
                foreach (IShape shape in _viewModel.ShapeList)
                {
                    _viewModel.RenderList.Add(shape.Draw());
                    if (shape.isSelected) shape.ShowAdorner();
                }

            }
        }
    }
}
