using Custom_Paint.Helper;
using Custom_Paint.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

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
                    var ui = shape.Draw();
                    this.StoreCanvas.Children.Add(ui);
                    ui.UpdateLayout();
                }
                this.StoreCanvas.UpdateLayout();
                PreviewUpdater.Update(this.StoreCanvas, this.MainCanvas);
            };
            viewModel.GetMainCanvasFunc = () => this.MainCanvas;

            // First update
            PreviewUpdater.Update(this.StoreCanvas, this.MainCanvas);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Polygon polygon = new Polygon();
            //polygon.Points.Clear();
            //polygon.StrokeThickness = 0;
            //polygon.Stroke = Brushes.Transparent;
            //polygon.Fill = Brushes.Red;
            //polygon.Points = new PointCollection()
            //{
            //    new Point(0, 50),
            //    new Point(50, 50),
            //    new Point(50, 0),
            //    new Point(0, 0),
            //    new Point(20, 20),
            //    new Point(10, 10),
            //};
            //this.PreviewCanvas.Children.Add(polygon);
        }
    }
}
