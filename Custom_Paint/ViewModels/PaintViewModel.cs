using Contract;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Input;
using Custom_Paint.Commands;
using Custom_Paint.Services;
using Point = System.Windows.Point;
using System.Windows.Controls;

namespace Custom_Paint.ViewModels
{
    public class PaintViewModel : ViewModelBase
    {
        // ======================================
        // Options
        // ======================================

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

        public string ChoosenShape { get; set; }
        private void ShapeButtonClick(object sender, RoutedEventArgs e)
        {
            var control = (Fluent.Button)sender;
            ChoosenShape = (string)control.Tag;
            if (PreviewObject != null)
            {
                this.AcceptPreview();
            }
        }

        public ICommand SizeButtonClick { get; }

        public bool FillMode { get; set; }
        public Func<Image> GetMainCanvasFunc;

        // ======================================
        // Handle
        // ======================================

        private Point _start;
        public Point Start { get { return _start; } set { _start = value; } }

        private Point _end;
        public Point End { get { return _end; } set { _end = value; } }

        private bool _isDrawing;
        public bool IsDrawing { get { return _isDrawing; } set { _isDrawing = value; } }

        public ICommand MouseDown { get; }
        public ICommand MouseUp { get; }
        public ICommand MouseMove { get; }

        // ======================================
        // Draw / Preview
        // ======================================

        private IShape? _previewObject;
        public IShape? PreviewObject
        {
            get { return _previewObject; }
            set
            {
                _previewObject = value;
            }
        }

        public UIElement? PreviewRender => _previewObject?.Draw();

        // ======================================
        // Storage
        // ======================================

        public ObservableCollection<IShape> StoredShapes { get; } = new ObservableCollection<IShape>();

        // ======================================
        // ======================================
        // ======================================

        public PaintViewModel()
        {
            this._start = new Point(0, 0);
            this._end = new Point(0, 0);
            this.ColorButtonClick = new ColorButtonClickCommand(this);

            //Handle Canvas
            this.MouseDown = new MouseDownCommand(this);
            this.MouseUp = new MouseUpCommand(this);
            this.MouseMove = new MouseMoveCommand(this);

            //options
            this.ListShapeButton = new List<Fluent.Button>();
            this.Factory = new ShapeFactory();
            this.SizeButtonClick = new SizeButtonClickCommand(this);
            this.FillCommand = new SimpleCommand(o =>
            {
                this.FillMode = !this.FillMode;
            });
            GetShapeButton();
        }

        public Action OnAcceptPreviewAction;
        public Action<UIElement?> OnRefreshPreviewAction;

        public ShapeFactory Factory { get; set; }
        public ICommand ColorButtonClick { get; }
        public ICommand FillCommand { get; }

        public void IgnorePreview()
        {
            if (this.PreviewObject != null)
            {
                this.PreviewObject.HideAdorner();
                this.PreviewObject = null;
                this.OnRefreshPreviewAction?.Invoke(null);
            }
        }

        public void AcceptPreview()
        {
            if (this.PreviewObject != null)
            {
                this.PreviewObject.HideAdorner();
                this.StoredShapes.Add(this.PreviewObject);
                this.PreviewObject = null;
                this.OnRefreshPreviewAction?.Invoke(null);
                this.OnAcceptPreviewAction.Invoke();
            }
        }

        public void PreviewUpdate()
        {
            if (PreviewRender != null)
            {
                OnRefreshPreviewAction?.Invoke(PreviewRender);
            }
        }

        public void PreviewUpdateWithEdit()
        {
            if (PreviewObject != null && PreviewObject.isSelected == true)
            {
                this.PreviewObject.HideAdorner();
                OnRefreshPreviewAction.Invoke(PreviewObject.Draw());
                this.PreviewObject.ShowAdorner();
            }
        }
    }
}


