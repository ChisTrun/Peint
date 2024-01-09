using Custom_Paint.Helper;
using Custom_Paint.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Custom_Paint.Views
{
    /// <summary>
    /// Interaction logic for PaintView.xaml
    /// </summary>
    public partial class PaintView : UserControl
    {

        public PaintView()
        {
            InitializeComponent();
            this.Loaded += PaintView_Loaded;
        }

        private void PaintView_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (PaintViewModel)this.DataContext;
            viewModel.OnRefreshPreviewAction = (ui) =>
            {
                this.PreviewCanvas.Children.Clear();
                if (ui != null)
                {
                    this.PreviewCanvas.Children.Add(ui);
                }
            };
            viewModel.OnAcceptPreviewAction = () =>
            {
                this.StoreCanvas.Children.Clear();
                foreach (var shape in viewModel.StoredShapes)
                {
                    this.StoreCanvas.Children.Add(shape.Draw());
                }
                PreviewUpdate.PreviewUpdater(this.StoreCanvas, this.MainCanvas);
            };
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (PaintViewModel)DataContext;
            if (e.ChangedButton == MouseButton.Left)
            {
                Point start = e.GetPosition(sender as Canvas);
                viewModel.MouseDown.Execute(start);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                viewModel.IgnorePreview();
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            var viewModel = (PaintViewModel)DataContext;
            viewModel.MouseMove.Execute(e.GetPosition(sender as Canvas));
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (PaintViewModel)DataContext;
            viewModel.MouseUp.Execute(sender);
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = (PaintViewModel)DataContext;
            if (e.Key == Key.Escape)
            {
                viewModel.IgnorePreview();
            }
        }

    }
}
