using Custom_Paint.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Custom_Paint.Commands
{
    public class FlipButtonClickCommand : CommandBase
    {
        private PaintViewModel _viewModel;

        public FlipButtonClickCommand(PaintViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public override void Execute(object? parameter)
        {
            if (parameter != null && _viewModel.PreviewObject != null)
            {
                string option = (string)parameter;
                if (option == "fv")
                {
                    _viewModel.PreviewObject.FlipV = _viewModel.PreviewObject.FlipV == -1 ? 1 : -1;
                }
                else
                {
                    _viewModel.PreviewObject.FlipH = _viewModel.PreviewObject.FlipH == -1 ? 1 : -1;
                }
                _viewModel.PreviewUpdateWithEdit();
            }
        }
    }
}
