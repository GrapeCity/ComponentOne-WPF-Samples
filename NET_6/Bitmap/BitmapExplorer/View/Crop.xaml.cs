using System;
using System.IO;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

using C1.WPF;
using C1.WPF.Bitmap;
using C1.Util.DX;
using System.Windows.Input;
using C1.WPF.Core;

namespace BitmapExplorer
{
    public partial class CropDemo : UserControl
    {
        C1Bitmap _bitmap;
        Point _start;
        C1DragHelper _dragHelper;
        Rect _selection;
        Window _window;
        bool _initialized;

        public CropDemo()
        {
            InitializeComponent();
            Tag = Properties.Resources.CropDemoDesc;

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
                _bitmap.Dispose();
            }
        }

        void LoadDefaultImage()
        {
            Assembly asm = typeof(CropDemo).Assembly;
            using (Stream stream = asm.GetManifestResourceStream("BitmapExplorer.Resources.GrapeCity.jpg"))
            {
                _bitmap.Load(stream, new FormatConverter(PixelFormat.Format32bppPBGRA));
            }
            image.Source = _bitmap.ToWriteableBitmap();
            imageGrid.Width = _bitmap.PixelWidth;
            imageGrid.Height = _bitmap.PixelHeight;
            InitSelection();
        }

        void LoadImage(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.ico;*.bmp;*.gif;*.png;*.jpg;*.jpeg;*.jxr;*.tif;*.tiff";
            ofd.Title = "Select the Image";

            if (ofd.ShowDialog().Value)
            {
                try
                {
                    _bitmap.Load(ofd.FileName, new FormatConverter(PixelFormat.Format32bppPBGRA));
                    image.Source = _bitmap.ToWriteableBitmap();
                    imageGrid.Width = _bitmap.PixelWidth;
                    imageGrid.Height = _bitmap.PixelHeight;

                    InitSelection();
                }
                catch (Exception ex)
                {
                    LoadDefaultImage();
                    MessageBox.Show(ex.Message);
                }
            }
        }

        void ExportImage(object sender, RoutedEventArgs e)
        {
            if (_selection.Width == 0 || _selection.Height == 0)
            {
                MessageBox.Show("Can't export, selection is empty.");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Png Files (*.png)|*.png";
            sfd.CheckPathExists = true;
            if (sfd.ShowDialog().Value)
            {
                var cropRect = ((RectD)_selection).Round();
                _bitmap.SaveAsPng(sfd.FileName, new PngOptions { SourceRect = new ImageRect(cropRect) });

                _selection = new Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight);
                UpdateMask();
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
