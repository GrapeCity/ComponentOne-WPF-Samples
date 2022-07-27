using System;
using System.IO;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

using C1.WPF;
using C1.WPF.Bitmap;
using C1.Util.DX;

namespace BitmapSamples
{
    /// <summary>
    /// Interaction logic for Transform.xaml
    /// </summary>
    public partial class Transform : UserControl
    {
        C1Bitmap _bitmap;
        C1Bitmap _savedCopy;
        Point _start;
        C1DragHelper _dragHelper;
        Rect _selection;
        Window _window;
        bool _initialized;

        public Transform()
        {
            InitializeComponent();

            Loaded += Page_Loaded;
            Unloaded += Page_Unloaded;
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_initialized)
            {
                _bitmap = new C1Bitmap();

                LoadDefaultImage();

                _dragHelper = new C1DragHelper(imageGrid);
                _dragHelper.DragDelta += OnDragDelta;

                _window = Window.GetWindow(this);
                if (_window != null)
                {
                    _window.Closing += Window_Closing;
                }
                _initialized = true;
            }
        }

        void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            CleanUp();
        }

        void Window_Closing(object sender, CancelEventArgs e)
        {
            CleanUp();
        }

        void CleanUp()
        {
            if (_initialized)
            {
                if (_window != null)
                {
                    _window.Closing -= Window_Closing;
                }
                _initialized = false;
                _dragHelper.FinalizeHelper();
                image.Source = null;
                ClearSavedCopy();
                _bitmap.Dispose();
            }
        }

        void LoadDefaultImage()
        {
            Assembly asm = typeof(Crop).Assembly;
            using (Stream stream = asm.GetManifestResourceStream("BitmapSamples.Resources.HousePlan.jpg"))
            {
                _bitmap.Load(stream, new FormatConverter(PixelFormat.Format32bppPBGRA));
            }

            ClearSavedCopy();
            _savedCopy = _bitmap.Transform();

            UpdateImage();
        }

        void Load_Clicked(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.ico;*.bmp;*.gif;*.png;*.jpg;*.jpeg;*.jxr;*.tif;*.tiff";
            ofd.Title = "Select the Image";

            if (ofd.ShowDialog().Value)
            {
                try
                {
                    _bitmap.Load(ofd.FileName, new FormatConverter(PixelFormat.Format32bppPBGRA));

                    ClearSavedCopy();
                    _savedCopy = _bitmap.Transform();

                    UpdateImage();
                }
                catch (Exception ex)
                {
                    LoadDefaultImage();
                    MessageBox.Show(ex.Message);
                }
            }
        }

        void Export_Clicked(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Png Files (*.png)|*.png";
            sfd.CheckPathExists = true;
            if (sfd.ShowDialog().Value)
            {
                _bitmap.SaveAsPng(sfd.FileName, null);
                InitSelection();
            }
        }

        void Restart_Clicked(object sender, RoutedEventArgs e)
        {
            if (_savedCopy != null)
            {
                _bitmap.Dispose();
                _bitmap = _savedCopy.Transform();
                UpdateImage();
            }
        }

        void ClearSavedCopy()
        {
            if (_savedCopy != null)
            {
                _savedCopy.Dispose();
                _savedCopy = null;
            }
        }

        void ApplyTransform(BaseTransform t)
        {
            var newBitmap = _savedCopy.Transform(t);
            _bitmap.Dispose();
            _bitmap = newBitmap;
            UpdateImage();
        }

        void Crop_Clicked(object sender, RoutedEventArgs e)
        {
            var cropRect = ((RectD)_selection).Round();
            ApplyTransform(new Clipper(new ImageRect(cropRect)));
        }

        void RotateCCW_Clicked(object sender, RoutedEventArgs e)
        {
            ApplyTransform(new FlipRotator(TransformOptions.Rotate270));
        }

        void RotateCW_Clicked(object sender, RoutedEventArgs e)
        {
            ApplyTransform(new FlipRotator(TransformOptions.Rotate90));
        }

        void FlipH_Clicked(object sender, RoutedEventArgs e)
        {
            ApplyTransform(new FlipRotator(TransformOptions.FlipHorizontal));
        }

        void FlipV_Clicked(object sender, RoutedEventArgs e)
        {
            ApplyTransform(new FlipRotator(TransformOptions.FlipVertical));
        }

        void ScaleIn_Clicked(object sender, RoutedEventArgs e)
        {
            int px = (int)(_bitmap.PixelWidth * 1.6f + 0.5f);
            int py = (int)(_bitmap.PixelHeight * 1.6f + 0.5f);
            ApplyTransform(new Scaler(px, py, InterpolationMode.HighQualityCubic));
        }

        void ScaleOut_Clicked(object sender, RoutedEventArgs e)
        {
            int px = (int)(_bitmap.PixelWidth * 0.625f + 0.5f);
            int py = (int)(_bitmap.PixelHeight * 0.625f + 0.5f);
            if (px > 0 && py > 0)
            {
                ApplyTransform(new Scaler(px, py, InterpolationMode.HighQualityCubic));
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            var pos = e.GetPosition(imageGrid);
            _start = new Point(
                Math.Max(0, Math.Min(pos.X, _bitmap.PixelWidth)),
                Math.Max(0, Math.Min(pos.Y, _bitmap.PixelHeight)));
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            var pt = e.GetPosition(imageGrid);
            if (Math.Abs(pt.X - _start.X) < 4 && Math.Abs(pt.Y - _start.Y) < 4)
            {
                InitSelection();
            }
        }

        void OnDragDelta(object sender, C1DragDeltaEventArgs e)
        {
            var pos = e.GetPosition(imageGrid);
            pos = new Point(Math.Max(0, Math.Min(pos.X, _bitmap.PixelWidth)),
                Math.Max(0, Math.Min(pos.Y, _bitmap.PixelHeight)));

            _selection = new Rect(
                Math.Round(Math.Min(_start.X, pos.X)),
                Math.Round(Math.Min(_start.Y, pos.Y)),
                Math.Round(Math.Abs(_start.X - pos.X)),
                Math.Round(Math.Abs(_start.Y - pos.Y)));

            UpdateMask();
        }

        void UpdateImage()
        {
            image.Source = _bitmap.ToWriteableBitmap();
            imageGrid.Width = _bitmap.PixelWidth;
            imageGrid.Height = _bitmap.PixelHeight;
            InitSelection();
        }

        void InitSelection()
        {
            _selection = new Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight);
            UpdateMask();
        }

        void UpdateMask()
        {
            topMask.Height = _selection.Top;
            bottomMask.Height = _bitmap.PixelHeight - _selection.Bottom;
            leftMask.Width = _selection.Left;
            rightMask.Width = _bitmap.PixelWidth - _selection.Right;
        }
    }
}
