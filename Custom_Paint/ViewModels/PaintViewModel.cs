using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Input;
using Custom_Paint.Commands;
using Custom_Paint.Services;
using Point = System.Windows.Point;
using System.Windows.Controls;
using Contract;
using Custom_Paint.Helper;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using Custom_Paint.Contract;

namespace Custom_Paint.ViewModels
{
    public class PaintViewModel : ViewModelBase
    {
        // ======================================
        // Options
        // ======================================

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
                if (abilities.ObjType == ObjType.Shape)
                {
                    ListShapeButton.Add(button);
                }
                else if (abilities.ObjType == ObjType.Tool)
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
            if (PreviewObject != null)
            {
                this.AcceptPreview();
            }
        }

        public ICommand SizeButtonClick { get; }

        public ICommand StrokeDashButtonClick { get; }

        public ICommand FlipButtonClick { get; }
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
        // Storage
        // ======================================

        public ICommand ReadFile { get; }
        public ICommand WriteFile { get; }

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
            this.ListToolButton = new List<Fluent.Button>();
            this.Factory = new ShapeFactory();
            this.SizeButtonClick = new SizeButtonClickCommand(this);
            this.FillCommand = new SimpleCommand(o =>
            {
                this.FillMode = !this.FillMode;
            });
            this.StrokeDashButtonClick = new StrokeDashButtonClickCommand(this);
            this.FlipButtonClick = new FlipButtonClickCommand(this);
            GetAppAbilities();

            ReadFile = new SimpleCommand(o =>
            {
                string fileName = "";
                OpenFileDialog dlg = new OpenFileDialog();

                // Set filter for file extension and default file extension 
                dlg.DefaultExt = ".png";
                dlg.Filter = "Binary|*.bin|Text|*.txt|PNG|*.png|XML|*.xml|JSON|*.json";

                // Display OpenFileDialog by calling ShowDialog method 
                var result = dlg.ShowDialog();

                // Get the selected file name and display in a TextBox 
                if (result == DialogResult.OK)
                {
                    // Open document 
                    fileName = dlg.FileName;
                }
                var ext = Path.GetExtension(dlg.FileName);
                List<ShapeInfo> shapeInfos = new List<ShapeInfo>();
                if (ext == ".png")
                {
                    var image = PNGHelper.InsertImage(fileName);
                    double scale = 0.5;
                    image.LayoutTransform = new ScaleTransform(scale, scale);
                    this.StoredShapes.Clear();
                    this.StoredShapes.Add(new ImageShape(image));
                }
                else
                {
                    switch (ext)
                    {
                        case ".txt":
                            var reader1 = new ReaderText();
                            shapeInfos = reader1.Read(fileName);
                            break;
                        case ".bin":
                            var reader2 = new ReaderBinary();
                            shapeInfos = reader2.Read(fileName);
                            break;
                        case ".xml":
                            var reader3 = new ReaderXML();
                            shapeInfos = reader3.Read(fileName);
                            break;
                        case ".json":
                            var reader4 = new ReaderJSON();
                            shapeInfos = reader4.Read(fileName);
                            break;
                        default:
                            break;
                    }

                    this.StoredShapes.Clear();
                    shapeInfos.ForEach((s) =>
                    {
                        var name = s.Name;
                        var sh = Factory.prototypes[name].Clone();
                        sh.LoadInfo(s);
                        this.StoredShapes.Add(sh);
                    });
                }
                this.OnAcceptPreviewAction!.Invoke();
            });

            WriteFile = new SimpleCommand(o =>
            {
                string path = string.Empty;
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();
                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        path = fbd.SelectedPath;
                    }
                }
                switch (_selectedType)
                {
                    case ".txt":
                        var writer1 = new WriterText();
                        writer1.Write(path, StoredShapes.ToList());
                        break;
                    case ".bin":
                        var writer2 = new WriterBinary();
                        writer2.Write(path, StoredShapes.ToList());
                        break;
                    case ".xml":
                        var writer3 = new WriterXML();
                        writer3.Write(path, StoredShapes.ToList());
                        break;
                    case ".json":
                        var writer4 = new WriterJSON();
                        writer4.Write(path, StoredShapes.ToList());
                        break;
                    case ".png":
                        var image = this.GetMainCanvasFunc!.Invoke();
                        PNGHelper.SavePng(path, image);
                        break;
                    default:
                        break;
                }
            });
            _selectedType = SaveType[0];
            InsertImageCommand = new SimpleCommand(o =>
            {
                string fileName = "";
                OpenFileDialog dlg = new OpenFileDialog();

                // Set filter for file extension and default file extension 
                dlg.DefaultExt = ".png";
                dlg.Filter = "PNG|*.png";

                // Display OpenFileDialog by calling ShowDialog method 
                var result = dlg.ShowDialog();

                // Get the selected file name and display in a TextBox 
                if (result == DialogResult.OK)
                {
                    // Open document 
                    fileName = dlg.FileName;
                }
                var path = dlg.FileName;
                var image = PNGHelper.InsertImage(fileName);
                double scale = 0.5;
                image.LayoutTransform = new ScaleTransform(scale, scale);
                this.StoredShapes.Add(new ImageShape(image));
                this.OnAcceptPreviewAction!.Invoke();
            });
        }

        public ICommand InsertImageCommand { get; }

        public BindingList<string> SaveType { get; } = new BindingList<string>()
        {
            ".txt",
            ".bin",
            ".xml",
            ".json",
            ".png",
        };
        private string _selectedType;
        public string SelectedType { get { return _selectedType; } set { _selectedType = value; } }

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


