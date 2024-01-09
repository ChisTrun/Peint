﻿using Contract;
using Custom_Paint.ViewModels;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Custom_Paint.Commands
{
    public class MouseMoveCommand : CommandBase
    {

        private PaintViewModel _viewModel;

        public MouseMoveCommand(PaintViewModel vm)
        {
            _viewModel = vm;
        }

        public override void Execute(object? parameter)
        {
            if (parameter != null && _viewModel.IsDrawing)
            {
                _viewModel.RenderList.Clear();
                foreach (IShape shape in _viewModel.ShapeList)
                {
                    _viewModel.RenderList.Add(shape.Draw());
                }
                _viewModel.End = (Point)parameter;
                _viewModel.Preview.UpdatePoints(_viewModel.End);
                _viewModel.RenderList.Add(_viewModel.Preview.Draw());

            }
        }
    }
}
