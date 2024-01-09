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

        private string? _choosenShape;
        public string? ChoosenShape { get { return _choosenShape; } set { _choosenShape = value; } }
        private void ShapeButtonClick(object sender, RoutedEventArgs e)
        {
            var control = (Fluent.Button)sender;
            ChoosenShape = (string)control.Tag;
        }


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




        public ShapeFactory Factory { get; set; }
        public ICommand ColorButtonClick { get; }
        public List<Fluent.Button> ListShapeButton { get; set; }

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

            //Options
            this.ListShapeButton = new List<Fluent.Button>();
            this.Factory = new ShapeFactory();
            GetShapeButton();
        }
    }
}
