using Contract;
using Custom_Paint.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Custom_Paint.Commands
{
    public class ColorButtonClickCommand : CommandBase
    {
        private PaintViewModel _viewModel;

        public ColorButtonClickCommand(PaintViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            if (parameter != null)
            {
                _viewModel.CurrentColor = (SolidColorBrush)parameter;
                if (_viewModel.PreviewObject != null)
                {
                    _viewModel.PreviewObject.StrokeColor = _viewModel.CurrentColor;
                    _viewModel.PreviewUpdateWithEdit();
                }
            }
        }
    }
}
