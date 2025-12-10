using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Reflection;
using System.IO;
using Microsoft.Win32;

using C1.WPF;
using C1.WPF.Bitmap;

using D2D = C1.Util.DX.Direct2D;
using D3D = C1.Util.DX.Direct3D11;
using DXGI = C1.Util.DX.DXGI;
using WIC = C1.Util.DX.WIC;
using C1.Util.DX;
using C1.WPF.Core;

namespace BitmapExplorer
{
    public partial class FaceWarpDemo : UserControl
    {
        C1Bitmap _bitmap;
        C1Bitmap _savedCopy;
        C1DragHelper _dragHelper;
        Point _position;
        Line _line;
        Point2F _warpStart;
        Point2F _warpEnd;

        D2D.Factory2 _d2dFactory;
        WIC.ImagingFactory2 _wicFactory;

        DXGI.Device _dxgiDevice;
        D2D.DeviceContext1 _d2dContext;

        D2D.Effects.GaussianBlur _blur;
        WarpEffect _warp;

        Window _window;
        bool _initialized;

        public FaceWarpDemo()
        {
            InitializeComponent();
            Tag = Properties.Resources.FaceWrapDemoDesc;

            Loaded += Page_Loaded;
            Unloaded += Page_Unloaded;
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_initialized)
            {
                CreateDeviceIndependentResources();
                _bitmap = new C1Bitmap(_wicFactory);

                CreateDeviceResources();

                LoadDefaultImage();
                _dragHelper = new C1DragHelper(image, captureElementOnPointerPressed: true, initialThreshold: 0);
                _dragHelper.DragStarted += OnDragStarted;
                _dragHelper.DragDelta += OnDragDelta;
                _dragHelper.DragCompleted += OnDragCompleted;

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

                image.Source = null;
                _dragHelper.FinalizeHelper();

                DiscardDeviceResources();

                ClearSavedCopy();
                _bitmap.Dispose();

                DiscardDeviceIndependentResources();
            }
        }

        void OnDragStarted(object sender, C1DragStartedEventArgs e)
        {
            _position = e.GetPosition(image);
            _line = new Line
            {
                X1 = _position.X,
                Y1 = _position.Y,
                X2 = _position.X,
                Y2 = _position.Y,
                Stroke = new SolidColorBrush(Colors.Blue),
                StrokeThickness = 7,
                StrokeEndLineCap = PenLineCap.Triangle,
                StrokeStartLineCap = PenLineCap.Round
            };
            imageGrid.Children.Add(_line);
        }

        void OnDragDelta(object sender, C1DragDeltaEventArgs e)
        {
            var pos = e.GetPosition(image);
            _line.X2 = pos.X;
            _line.Y2 = pos.Y;
        }

        void OnDragCompleted(object sender, C1DragCompletedEventArgs e)
        {
            imageGrid.Children.Remove(_line);
            var start = new Point(_position.X + e.CumulativeTranslation.X, _position.Y + e.CumulativeTranslation.Y);
            var end = _position;

            var w = image.ActualWidth;
            var h = image.ActualHeight;
            _warpStart = new Point2F((float)(start.X / w), (float)(start.Y / h));
            _warpEnd = new Point2F((float)(end.X / w), (float)(end.Y / h));

            UpdateImageSource();
            image.Source = _bitmap.ToWriteableBitmap();
        }

        void LoadDefaultImage()
        {
            Assembly asm = typeof(FaceWarpDemo).Assembly;
            using (Stream stream = asm.GetManifestResourceStream("BitmapExplorer.Resources.Sheep.jpg"))
            {
                _bitmap.Load(stream, new FormatConverter(C1.WPF.Bitmap.PixelFormat.Format32bppPBGRA));
            }

            ClearSavedCopy();
            _savedCopy = _bitmap.Transform();

            image.Source = _bitmap.ToWriteableBitmap();
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
                    _bitmap.Load(ofd.FileName, new FormatConverter(C1.WPF.Bitmap.PixelFormat.Format32bppPBGRA));
                    image.Source = _bitmap.ToWriteableBitmap();

                    ClearSavedCopy();
                    _savedCopy = _bitmap.Transform();
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
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Png Files (*.png)|*.png";
            sfd.CheckPathExists = true;
            if (sfd.ShowDialog().Value)
            {
                _bitmap.SaveAsPng(sfd.FileName, null);
            }
        }

        void Restart(object sender, RoutedEventArgs e)
        {
            _bitmap.ImportAsFragment(_savedCopy, 0, 0);
            image.Source = _bitmap.ToWriteableBitmap();
        }

        void ClearSavedCopy()
        {
            if (_savedCopy != null)
            {
                _savedCopy.Dispose();
                _savedCopy = null;
            }
        }

        void CreateDeviceIndependentResources()
        {
            _d2dFactory = D2D.Factory2.Create(D2D.FactoryType.SingleThreaded);
            _wicFactory = WIC.ImagingFactory2.Create();
        }

        void DiscardDeviceIndependentResources()
        {
            DXUtil.Dispose(ref _d2dFactory);
            DXUtil.Dispose(ref _wicFactory);
        }

        void CreateDeviceResources()
        {
            D3D.FeatureLevel actualLevel;
            D3D.DeviceContext d3dContext = null;
            var d3dDevice = new D3D.Device(IntPtr.Zero);
            var result = HResult.Ok;

            for (int i = 0; i <= 1; i++)
            {
                // use WARP if hardware is not available
                var dt = i == 0 ? D3D.DriverType.Hardware : D3D.DriverType.Warp;
                result = D3D.D3D11.CreateDevice(null, dt, IntPtr.Zero, D3D.DeviceCreationFlags.BgraSupport | D3D.DeviceCreationFlags.SingleThreaded,
                    null, 0, D3D.D3D11.SdkVersion, d3dDevice, out actualLevel, out d3dContext);
                if (result.Code != unchecked((int)0x887A0004)) // DXGI_ERROR_UNSUPPORTED
                {
                    break;
                }
            }
            result.CheckError();
            d3dContext.Dispose();

            _dxgiDevice = d3dDevice.QueryInterface<DXGI.Device>();
            d3dDevice.Dispose();

            var d2dDevice = D2D.Device1.Create(_d2dFactory, _dxgiDevice);
            _d2dContext = d2dDevice.CreateDeviceContext1(D2D.DeviceContextOptions.None);
            _d2dContext.SetUnitMode(D2D.UnitMode.Pixels);
            d2dDevice.Dispose();

            var sourceEffect = D2D.Effect.RegisterAndCreateCustom<WarpEffect>(_d2dFactory, _d2dContext);
            _warp = (WarpEffect)sourceEffect.CustomEffect;
            _warp.Effect = sourceEffect;

            _blur = D2D.Effects.GaussianBlur.Create(_d2dContext);
            _blur.StandardDeviation = 2f;
        }

        internal void DiscardDeviceResources()
        {
            DXUtil.Dispose(ref _blur);
            DXUtil.Dispose(ref _warp);

            DXUtil.Dispose(ref _dxgiDevice);
            DXUtil.Dispose(ref _d2dContext);
        }

        void UpdateImageSource()
        {
            int w = _bitmap.PixelWidth;
            int h = _bitmap.PixelHeight;

            var rt = _d2dContext;

            var bpTarget = new D2D.BitmapProperties1(
                new D2D.PixelFormat(DXGI.Format.B8G8R8A8_UNorm, D2D.AlphaMode.Premultiplied),
                (float)_bitmap.DpiX, (float)_bitmap.DpiY, D2D.BitmapOptions.Target | D2D.BitmapOptions.CannotDraw);

            var targetBmp = D2D.Bitmap1.Create(rt, new Size2L(w, h), bpTarget);

            rt.SetTarget(targetBmp);

            rt.BeginDraw();
            rt.Clear(null);

            var buffer = _bitmap.ToD2DBitmap1(rt, D2D.BitmapOptions.None);

            _warp.SetNormPositions(_warpStart, _warpEnd);
            _warp.Effect.SetInput(0, buffer);

            rt.DrawImage(_warp.Effect, Point2F.Empty);

            //_blur.SetInputEffect(0, _warp.Effect);
            //rt.DrawImage(_blur, Point2F.Empty);

            buffer.Dispose();

            if (!rt.EndDraw(true))
            {
                targetBmp.Dispose();
                DiscardDeviceResources();
                CreateDeviceResources();
                return;
            }
            rt.SetTarget(null);

            _bitmap.ImportAsFragment(targetBmp, rt, new RectL(w, h), 0, 0);

            targetBmp.Dispose();
        }
    }
}
