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
            if (parameter != null && _viewModel.Preview != null)
            {
                _viewModel.CurrentStrokeThickness = (double)parameter;
                _viewModel.Preview.StrokeThickness = _viewModel.CurrentStrokeThickness;
                _viewModel.PreviewRender = _viewModel.Preview.Draw();
                _viewModel.Preview.ShowAdorner();
            }
        }
    }
}
