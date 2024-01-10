using Contract;
using Custom_Paint.ViewModels;
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
            if (_viewModel.IsDrawing && _viewModel.PreviewObject != null)
            {
                _viewModel.IsDrawing = false;
                _viewModel.PreviewObject.ShowAdorner();
                // Insert / for nor-edtable objects (show adorners will set isSelected of editable -> true)
                if (_viewModel.PreviewObject.isSelected == false)
                {
                    _viewModel.AcceptPreview();
                }
            }
        }
    }
}
