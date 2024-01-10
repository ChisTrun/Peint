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
            if(parameter != null && _viewModel.Preview != null)
            {
                string option = (string)parameter;
                if(option == "fv")
                {
                    _viewModel.Preview.FlipV = _viewModel.Preview.FlipV == -1? 1 : -1;
                    _viewModel.PreviewRender = _viewModel.Preview.Draw();
                    _viewModel.Preview.ShowAdorner();
                } else
                {
                    _viewModel.Preview.FlipH = _viewModel.Preview.FlipH == -1 ? 1 : -1;
                    _viewModel.PreviewRender = _viewModel.Preview.Draw();
                    _viewModel.Preview.ShowAdorner();
                }
            }
        }
    }
}
