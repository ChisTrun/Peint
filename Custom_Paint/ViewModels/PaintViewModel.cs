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
        // Options
        public List<Fluent.Button> ListShapeButton { get; set; }
        public List<Fluent.Button> ListToolButton { get; set; }
        public List<Fluent.Button> ListSizeButton { get; set; }

        public List<double> ListStrokeSize { get; set; } = new List<double>() { 1, 3, 5, 8 };

        private DoubleCollection _currentStrokeDashArray = new DoubleCollection(new double[] { });

        public DoubleCollection CurrentStrokeDashArray { get { return _currentStrokeDashArray; } set { _currentStrokeDashArray = value; } }
        public List<DoubleCollection> ListStrokeDash { get; set; } = new List<DoubleCollection>
        {
            new DoubleCollection(new double[] {}),
            new DoubleCollection(new double[]{1}),
            new DoubleCollection(new double[]{ 2,5}),
            new DoubleCollection(new double[]{ 1,3,4})
        };

        private SolidColorBrush _currentColor = System.Windows.Media.Brushes.Black;
        public SolidColorBrush CurrentColor { get { return _currentColor; } set { _currentColor = value; } }

        private double _currentStrokeThickness = 1;

        public double CurrentStrokeThickness { get { return _currentStrokeThickness; } set { _currentStrokeThickness = value; } }
        private void GetAppAbilities()
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
                button.Click += AbilitiesClick;
                if (abilities.ObjType == Contract.Type.Shape)
                {
                    ListShapeButton.Add(button);
                } else if (abilities.ObjType == Contract.Type.Tool)
                {
                    ListToolButton.Add(button);
                }
            }
        }

        public string ChoosenShape { get; set; }
        private void AbilitiesClick(object sender, RoutedEventArgs e)
        {
            var control = (Fluent.Button)sender;
            ChoosenShape = (string)control.Tag;
            if (Preview != null)
            {
                this.Preview.HideAdorner();
                this.AcceptReview.Invoke(this.Preview.Draw());
                this.Preview = null;
            }
        }
        public ICommand SizeButtonClick { get; }

        public ICommand StrokeDashButtonClick { get; }

        // Handle

        private Point _start;
        public Point Start { get { return _start; } set { _start = value; } }

        private Point _end;
        public Point End { get { return _end; } set { _end = value; } }


        private bool _isDrawing;
        public bool IsDrawing { get { return _isDrawing; } set { _isDrawing = value; } }



        public ICommand MouseDown { get; }
        public ICommand MouseUp { get; }
        public ICommand MouseMove { get; }


        //Draw / Preview

        private IShape? _preview;
        public IShape? Preview
        {
            get { return _preview; }
            set
            {
                _preview = value;
            }
        }

        public UIElement _previewRender;
        public UIElement PreviewRender
        {
            get
            {
                return _previewRender;
            }
            set
            {
                _previewRender = value;
                RefreshReview.Invoke(_previewRender);
            }
        }

        public Action<UIElement> RefreshReview;
        public Action<UIElement> AcceptReview;
        public Func<List<UIElement>> GetStorage;

        public List<UIElement> GetData()
        {
            return GetStorage.Invoke();
        }

        //public List<UIElement> Storage { get; set; }



        public ShapeFactory Factory { get; set; }
        public ICommand ColorButtonClick { get; }

        public PaintViewModel()
        {
            this._start = new Point(0, 0);
            this._end = new Point(0, 0);
            this._isDrawing = false;
            this.ColorButtonClick = new ColorButtonClickCommand(this);

            //Handle Canvas
            this.MouseDown = new MouseDownCommand(this);
            this.MouseUp = new MouseUpCommand(this);
            this.MouseMove = new MouseMoveCommand(this);

            //options
            this.ListShapeButton = new List<Fluent.Button>();
            this.ListToolButton = new List<Fluent.Button>();
            this.Factory = new ShapeFactory();
            this.SizeButtonClick = new SizeButtonClickCommand(this);
            this.StrokeDashButtonClick = new StrokeDashButtonClickCommand(this);
            GetAppAbilities();
        }

    }
}
