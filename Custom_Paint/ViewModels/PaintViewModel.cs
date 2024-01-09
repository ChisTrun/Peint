using Contract;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Input;
using Custom_Paint.Commands;
using Custom_Paint.Services;
using System.Windows.Media.Imaging;

namespace Custom_Paint.ViewModels
{
    public class PaintViewModel : ViewModelBase
    {

        //private BitmapImage _bitMap;
        //public BitmapImage BitMap {  get { return _bitMap; } set { _bitMap = value; } }

        // Options
        private SolidColorBrush _currentColor = Brushes.Black;
        public SolidColorBrush CurrentColor { get { return _currentColor; } set { _currentColor = value; } }

        private void GetShapeButton()
        {
            string folder = AppDomain.CurrentDomain.BaseDirectory + "ShapeLib\\";
            var shapeAbilities = DllReader<IShape>.GetAbilities(folder);

            foreach (var abilities in shapeAbilities)
            {
                Factory.prototypes.Add(abilities.Name, abilities);
                Fluent.Button button = new Fluent.Button()
                {
                    Header = abilities.Icon,
                    Tag = abilities.Name,
                };
                button.Click += ShapeButtonClick;
                ListShapeButton.Add(button);
            }
        }

        private void ShapeButtonClick(object sender, RoutedEventArgs e)
        {
            var control = (Fluent.Button)sender;
            Preview = Factory.CreateShape((string)control.Tag);
        }

        //public UIElement? _selectedElement = null;


        // Handle
        private Point _start;
        public Point Start { get { return _start; } set { _start = value; } }

        private Point _end;
        public Point End { get { return _end; } set { _end = value; } }


        private bool _isDrawing;
        public bool IsDrawing { get { return _isDrawing; } set { _isDrawing = value; } }

        private IShape? _preview;
        public IShape? Preview { get { return _preview; } set { _preview = value; } }

        public ICommand MouseDown { get; }
        public ICommand MouseUp { get; }
        public ICommand MouseMove { get; }


        //Draw / Preview

        public List<IShape> ShapeList = new List<IShape>();
        public ObservableCollection<UIElement> RenderList { get; set; } //

        // >>

        public UIElement PreviewRender { get; set; }

        public Action<UIElement> RefreshReview;

        // <<

            
        public ShapeFactory Factory { get; set; }

        public ICommand ColorButtonClick{ get; }

        public List<Fluent.Button> ListShapeButton {  get; set; }   

        

        public PaintViewModel()
        {
            this._start = new Point(0, 0);
            this._end = new Point(0, 0);
            this._isDrawing = false;
            this.ColorButtonClick = new ColorButtonClickCommand(this);
            //Mouse event
            this.MouseDown = new MouseDownCommand(this);
            this.MouseUp = new MouseUpCommand(this);
            this.MouseMove = new MouseMoveCommand(this);


            this.RenderList = new ObservableCollection<UIElement>() {};
            //options
            this.ListShapeButton = new List<Fluent.Button>();
            this.Factory = new ShapeFactory();
            GetShapeButton();
        }





        //private void Canvas_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (_isDrawing)
        //    {
        //        RefeshCanvas();
        //        _end = e.GetPosition(DrawCanvas);
        //        _preview.UpdatePoints(_end);
        //        DrawCanvas.Children.Add(_preview.Draw());
        //    }
        //}

        //private void line_button(object sender, RoutedEventArgs e)
        //{
        //    Preview = new Line2D();
        //}

        //private void rect_button(object sender, RoutedEventArgs e)
        //{
        //    Preview = new Rect2D();
        //}

        //private void DrawCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.Source.ToString() == "System.Windows.Controls.Canvas")
        //    {
        //        _selectedElement = null;
        //        IEnumerable<UIElement> listUielement = DrawCanvas.Children.OfType<UIElement>();
        //        foreach (var item in listUielement)
        //        {
        //            IShape.RemoveResize(item);
        //        }
        //        foreach (var item in _lines) item.isSelected = false;
        //    }
        //}

        //private void RibbonWindow_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Delete)
        //    {
        //        List<IShape> removeList = new List<IShape>();
        //        foreach (var item in _lines)
        //        {
        //            if (item.isSelected) removeList.Add(item);
        //        }
        //        foreach (var item in removeList)
        //        {
        //            _lines.Remove(item);
        //        }
        //        RefeshCanvas();
        //    }
        //}

        //private void RefeshCanvas()
        //{
        //    DrawCanvas.Children.Clear();
        //    foreach (IShape shape in _lines)
        //    {
        //        DrawCanvas.Children.Add(shape.Draw());
        //    }
        //}



    }
}
