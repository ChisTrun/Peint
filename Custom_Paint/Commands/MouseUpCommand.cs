using Contract;
using Custom_Paint.ViewModels;
using RectShape;
using System.Drawing;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Custom_Paint.Commands
{
    public class MouseUpCommand : CommandBase
    {

        private PaintViewModel _viewModel;

        public MouseUpCommand(PaintViewModel vm)
        {
            _viewModel = vm;
        }
        public override void Execute(object? parameter)
        {
            if(_viewModel.IsDrawing)
            {
                _viewModel.IsDrawing = false;
                _viewModel.ShapeList.Add(_viewModel.Preview); // Will be deleted 
                _viewModel.Preview.ShowAdorner();
                _viewModel.Preview = _viewModel.Preview.Clone();

            }
        }

      
    }
}
