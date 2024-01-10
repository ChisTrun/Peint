using Custom_Paint.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Custom_Paint.Commands
{
    public class StrokeDashButtonClickCommand : CommandBase
    {
        private PaintViewModel _viewModel;

        public StrokeDashButtonClickCommand(PaintViewModel? viewModel)
        {
            _viewModel = viewModel;
        }
    
        public override void Execute(object? parameter)
        {
            if(parameter != null && _viewModel.Preview != null)
            {
                _viewModel.CurrentStrokeDashArray = (DoubleCollection)parameter;
                _viewModel.Preview.StrokeDashArray = _viewModel.CurrentStrokeDashArray;
                _viewModel.PreviewRender = _viewModel.Preview.Draw();
                _viewModel.Preview.ShowAdorner();
            }
            
        }
    }
}
