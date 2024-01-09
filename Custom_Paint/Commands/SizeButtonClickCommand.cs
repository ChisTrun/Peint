﻿using Contract;
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
            if (parameter != null && _viewModel.PreviewObject != null)
            {
                _viewModel.CurrentStrokeThickness = (double)parameter;
                _viewModel.PreviewObject.StrokeThickness = _viewModel.CurrentStrokeThickness;
                _viewModel.PreviewUpdateWithEdit();
            }
        }
    }
}
