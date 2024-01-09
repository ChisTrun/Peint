using Contract;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Input;
using Custom_Paint.Commands;
using Custom_Paint.Services;
using System.Drawing;
using Point = System.Windows.Point;

namespace Custom_Paint.ViewModels
{
    public class PaintViewModel : ViewModelBase
    {

        //private BitmapImage _bitMap;
        //public BitmapImage BitMap {  get { return _bitMap; } set { _bitMap = value; } }

        // Options
        public List<Fluent.Button> ListShapeButton { get; set; }
        public List<Fluent.Button> ListSizeButton { get; set; }

        public List<double> ListStrokeSize { get; set; } = new List<double>() { 1, 3, 5, 8 };



        private SolidColorBrush _currentColor = System.Windows.Media.Brushes.Black;
        public SolidColorBrush CurrentColor { get { return _currentColor; } set { _currentColor = value; } }

        private double _currentStrokeThickness = 1;

        public double CurrentStrokeThickness { get { return _currentStrokeThickness; } set { _currentStrokeThickness = value; } }
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
        public ICommand SizeButtonClick { get; }




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

        public ICommand ColorButtonClick { get; }





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


            this.RenderList = new ObservableCollection<UIElement>() { };
            //options
            this.ListShapeButton = new List<Fluent.Button>();
            this.Factory = new ShapeFactory();
            this.SizeButtonClick = new SizeButtonClickCommand(this);
            GetShapeButton();
        }

    }
}
