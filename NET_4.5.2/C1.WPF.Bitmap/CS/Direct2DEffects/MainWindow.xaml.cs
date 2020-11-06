using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

using C1.WPF.Bitmap;

using D2D = C1.Util.DX.Direct2D;
using D3D = C1.Util.DX.Direct3D11;
using DW = C1.Util.DX.DirectWrite;
using DXGI = C1.Util.DX.DXGI;
using C1.Util.DX;

namespace Direct2DEffects
{
    /// <summary>
    /// Effects that can be applied to the image.
    /// </summary>
    public enum ImageEffect
    {
        Original,
        GaussianBlur,
        Sharpen,
        HorizontalSmear,
        Shadow,
        DisplacementMap,
        Emboss,
        EdgeDetect,
        Sepia
    }

    /// <summary>
    /// Represents an item of the "Effects" ComboBox.
    /// </summary>
    public class EffectItem
    {
        public string EffectName { get; set; }
        public ImageEffect ImageEffect { get; set; }

        public EffectItem(ImageEffect imageEffect, string effectName)
        {
            ImageEffect = imageEffect;
            EffectName = effectName;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        C1Bitmap _bitmap;

        // device-independent resources
        D2D.Factory2 _d2dFactory;
        DW.Factory _dwFactory;

        // device resources
        DXGI.Device _dxgiDevice;
        D2D.DeviceContext1 _d2dContext;
        D2D.SolidColorBrush _brush;
        DW.TextLayout1 _textLayout;

        // Direct2D built-in effects
        D2D.Effects.Shadow _shadow;
        D2D.Effects.AffineTransform2D _affineTransform;
        D2D.Effects.Composite _composite;
        D2D.Effects.DisplacementMap _displacementMap;
        D2D.Effects.Turbulence _turbulence;
        D2D.Effects.Tile _tile;
        D2D.Effects.GaussianBlur _gaussianBlur;
        D2D.Effects.ConvolveMatrix _convolveMatrix;
        D2D.Effects.ColorMatrix _colorMatrix;
        D2D.Effects.Flood _flood;
        D2D.Effects.Crop _crop;

        const int _marginLT = 20;
        const int _marginRB = 36;

        public MainWindow()
        {
            InitializeComponent();

            InitResources();
            Closed += (s, e) => CleanUp();

            // fill the list of available effects
            var effectList = new List<EffectItem>();
            effectList.Add(new EffectItem(ImageEffect.Original, "Original Image"));
            effectList.Add(new EffectItem(ImageEffect.GaussianBlur, "Gaussian Blur"));
            effectList.Add(new EffectItem(ImageEffect.Sharpen, "Sharpen"));
            effectList.Add(new EffectItem(ImageEffect.HorizontalSmear, "Horizontal Smear"));
            effectList.Add(new EffectItem(ImageEffect.Shadow, "Shadow"));
            effectList.Add(new EffectItem(ImageEffect.DisplacementMap, "Displacement Map"));
            effectList.Add(new EffectItem(ImageEffect.Emboss, "Emboss"));
            effectList.Add(new EffectItem(ImageEffect.EdgeDetect, "Edge Detect"));
            effectList.Add(new EffectItem(ImageEffect.Sepia, "Sepia"));
            effectsCombo.ItemsSource = effectList;
            effectsCombo.SelectedIndex = 0;
        }

        void effectsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (EffectItem)e.AddedItems[0];
            UpdateImageSource(item.ImageEffect);
        }

        void Export_Click(object sender, RoutedEventArgs e)
        {
            ExportToGrayscale();
        }

        void InitResources()
        {
            // load the image into C1Bitmap
            _bitmap = new C1Bitmap();
            Assembly asm = typeof(MainWindow).Assembly;
            using (Stream stream = asm.GetManifestResourceStream("Direct2DEffects.Resources.GcLogo.png"))
            {
                _bitmap.Load(stream, new FormatConverter(C1.WPF.Bitmap.PixelFormat.Format32bppPBGRA));
            }

            // create Direct2D and DirectWrite factories
            _d2dFactory = D2D.Factory2.Create(D2D.FactoryType.SingleThreaded);
            _dwFactory = DW.Factory.Create(DW.FactoryType.Shared);

            // create GPU resources
            CreateDeviceResources();
        }

        void CleanUp()
        {
            DiscardDeviceResources();

            _bitmap.Dispose();
            _d2dFactory.Dispose();
            _dwFactory.Dispose();
        }

        /// <summary>
        /// Creates and initializes GPU resources.
        /// </summary>
        void CreateDeviceResources()
        {
            // create the Direct3D device
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

            // store the DXGI device (for trimming when the application is being suspended)
            _dxgiDevice = d3dDevice.QueryInterface<DXGI.Device>();
            d3dDevice.Dispose();

            // create a RenderTarget (DeviceContext for Direct2D drawing)
            var d2dDevice = D2D.Device1.Create(_d2dFactory, _dxgiDevice);
            var rt = D2D.DeviceContext1.Create(d2dDevice, D2D.DeviceContextOptions.None);
            d2dDevice.Dispose();
            rt.SetUnitMode(D2D.UnitMode.Pixels);
            _d2dContext = rt;

            // create built-in effects
            _shadow = D2D.Effects.Shadow.Create(rt);
            _affineTransform = D2D.Effects.AffineTransform2D.Create(rt);
            _composite = D2D.Effects.Composite.Create(rt);
            _displacementMap = D2D.Effects.DisplacementMap.Create(rt);
            _turbulence = D2D.Effects.Turbulence.Create(rt);
            _tile = D2D.Effects.Tile.Create(rt);
            _gaussianBlur = D2D.Effects.GaussianBlur.Create(rt);
            _convolveMatrix = D2D.Effects.ConvolveMatrix.Create(rt);
            _colorMatrix = D2D.Effects.ColorMatrix.Create(rt);
            _flood = D2D.Effects.Flood.Create(rt);
            _crop = D2D.Effects.Crop.Create(rt);

            // create a brush and TextLayout for drawing text on the image
            _brush = D2D.SolidColorBrush.Create(rt, ColorF.Red);

            var format = DW.TextFormat.Create(_dwFactory, "Gabriola", DW.FontWeight.Bold, DW.FontStyle.Normal, 28f);
            _textLayout = DW.TextLayout1.Create(_dwFactory, "Direct2D Effects", format, 350f, 100f);
            format.Dispose();

            // add a font feature for more beautiful text
            var typo = DW.Typography.Create(_dwFactory);
            var fontFeature = new DW.FontFeature(DW.FontFeatureTag.StylisticSet7, 1);
            typo.AddFontFeature(fontFeature);
            _textLayout.SetTypography(typo, new DW.TextRange(0, 100));
            typo.Dispose();
        }

        /// <summary>
        /// Releases GPU resources.
        /// </summary>
        void DiscardDeviceResources()
        {
            _shadow.Dispose();
            _affineTransform.Dispose();
            _composite.Dispose();
            _displacementMap.Dispose();
            _turbulence.Dispose();
            _tile.Dispose();
            _gaussianBlur.Dispose();
            _convolveMatrix.Dispose();
            _colorMatrix.Dispose();
            _flood.Dispose();
            _crop.Dispose();

            _textLayout.Dispose();
            _brush.Dispose();
            _dxgiDevice.Dispose();
            _d2dContext.Dispose();
        }

        void UpdateImageSource(ImageEffect imageEffect)
        {
            // some effects can change pixels outside the bounds of the source
            // image, so we need a margin to make those pixels visible
            var targetOffset = new Point2F(_marginLT, _marginLT);
            int w = _bitmap.PixelWidth + _marginLT + _marginRB;
            int h = _bitmap.PixelHeight + _marginLT + _marginRB;

            // the render target object
            var rt = _d2dContext;

            // create the target Direct2D bitmap
            var bpTarget = new D2D.BitmapProperties1(
                new D2D.PixelFormat(DXGI.Format.B8G8R8A8_UNorm, D2D.AlphaMode.Premultiplied),
                (float)_bitmap.DpiX, (float)_bitmap.DpiY, D2D.BitmapOptions.Target | D2D.BitmapOptions.CannotDraw);
            var targetBmp = D2D.Bitmap1.Create(rt, new Size2L(w, h), bpTarget);

            // associate the target bitmap with render target
            rt.SetTarget(targetBmp);

            // start drawing
            rt.BeginDraw();

            // clear the target bitmap
            rt.Clear(null);

            // convert C1Bitmap image to Direct2D image
            var d2dBitmap = _bitmap.ToD2DBitmap1(rt, D2D.BitmapOptions.None);

            // apply the effect or just draw the original image
            switch (imageEffect)
            {
                case ImageEffect.Original:
                    rt.DrawImage(d2dBitmap, targetOffset);
                    break;
                case ImageEffect.GaussianBlur:
                    rt.DrawImage(ApplyGaussianBlur(d2dBitmap), targetOffset);
                    break;
                case ImageEffect.Sharpen:
                    rt.DrawImage(ApplySharpen(d2dBitmap), targetOffset);
                    break;
                case ImageEffect.HorizontalSmear:
                    rt.DrawImage(ApplyHorizontalSmear(d2dBitmap), targetOffset);
                    break;
                case ImageEffect.Shadow:
                    rt.DrawImage(ApplyShadow(d2dBitmap), targetOffset);
                    break;
                case ImageEffect.DisplacementMap:
                    rt.DrawImage(ApplyDisplacementMap(d2dBitmap), targetOffset);
                    break;
                case ImageEffect.Emboss:
                    rt.DrawImage(ApplyEmboss(d2dBitmap), targetOffset);
                    break;
                case ImageEffect.EdgeDetect:
                    rt.DrawImage(ApplyEdgeDetect(d2dBitmap), targetOffset);
                    break;
                case ImageEffect.Sepia:
                    rt.DrawImage(ApplySepia(d2dBitmap), targetOffset);
                    break;
            }
            d2dBitmap.Dispose();

            // draw the text label in case of the Shadow effect
            if (imageEffect == ImageEffect.Shadow)
            {
                var mr = Matrix3x2.Rotation(-90f);
                var mt = Matrix3x2.Translation(targetOffset.X + 6f, targetOffset.Y + 344f);
                rt.Transform = mr * mt;
                _brush.SetColor(ColorF.White);
                rt.DrawTextLayout(new Point2F(-1f, -1f), _textLayout, _brush);
                _brush.SetColor(ColorF.DimGray);
                rt.DrawTextLayout(Point2F.Empty, _textLayout, _brush);
                rt.Transform = Matrix3x2.Identity;
            }

            // finish drawing (all drawing commands are executed at that moment)
            if (!rt.EndDraw(true))
            {
                targetBmp.Dispose();

                // try to recreate the device resources if the old GPU device was removed
                DiscardDeviceResources();
                CreateDeviceResources();
                return;
            }

            // detach the target bitmap
            rt.SetTarget(null);

            // create a temporary C1Bitmap object
            var outBitmap = new C1Bitmap(_bitmap.ImagingFactory);

            // import the image from Direct2D target bitmap to C1Bitmap
            outBitmap.Import(targetBmp, rt, new RectL(w, h));
            targetBmp.Dispose();

            // convert C1Bitmap to a WriteableBitmap, then use it as an image source
            image.Source = outBitmap.ToWriteableBitmap();
            outBitmap.Dispose();
        }

        D2D.Effect ApplyGaussianBlur(D2D.Bitmap1 bitmap)
        {
            _gaussianBlur.SetInput(0, bitmap);
            _gaussianBlur.BorderMode = D2D.BorderMode.Soft;
            _gaussianBlur.StandardDeviation = 3f;
            return _gaussianBlur;
        }

        D2D.Effect ApplySharpen(D2D.Bitmap1 bitmap)
        {
            _convolveMatrix.SetInput(0, bitmap);
            _convolveMatrix.KernelSizeX = 3;
            _convolveMatrix.KernelSizeY = 3;
            _convolveMatrix.KernelMatrix = new float[]
            {
                 0.0f, -1.0f,  0.0f,
                -1.0f,  6.0f, -1.0f,
                 0.0f, -1.0f,  0.0f
            };
            _convolveMatrix.Divisor = 2f;
            return _convolveMatrix;
        }

        D2D.Effect ApplyHorizontalSmear(D2D.Bitmap1 bitmap)
        {
            _convolveMatrix.SetInput(0, bitmap);
            _convolveMatrix.KernelSizeX = 20;
            _convolveMatrix.KernelSizeY = 1;
            _convolveMatrix.KernelMatrix = new float[]
            {
                1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f,
                1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f
            };
            _convolveMatrix.Divisor = 20f;
            return _convolveMatrix;
        }

        D2D.Effect ApplyShadow(D2D.Bitmap1 bitmap)
        {
            _shadow.SetInput(0, bitmap);
            _shadow.BlurStandardDeviation = 5f;
            _affineTransform.SetInputEffect(0, _shadow);
            _affineTransform.TransformMatrix = Matrix3x2.Translation(20f, 20f);
            _composite.SetInputEffect(0, _affineTransform);
            _composite.SetInput(1, bitmap);
            return _composite;
        }

        D2D.Effect ApplyDisplacementMap(D2D.Bitmap1 bitmap)
        {
            _displacementMap.SetInput(0, bitmap);
            _turbulence.Stitchable = true;
            _tile.SetInputEffect(0, _turbulence);
            _tile.Rectangle = new Vector4(0, 0, 512, 512);
            _displacementMap.SetInputEffect(1, _tile);
            _displacementMap.Scale = 100f;
            return _displacementMap;
        }

        D2D.Effect ApplyEmboss(D2D.Bitmap1 bitmap)
        {
            _convolveMatrix.SetInput(0, bitmap);
            _convolveMatrix.KernelSizeX = 3;
            _convolveMatrix.KernelSizeY = 3;
            _convolveMatrix.KernelMatrix = new float[]
            {
                2.0f,  1.0f,  0.0f,
                1.0f,  1.0f, -1.0f,
                0.0f, -1.0f, -2.0f
            };
            _convolveMatrix.Divisor = 1f;
            return _convolveMatrix;
        }

        D2D.Effect ApplyEdgeDetect(D2D.Bitmap1 bitmap)
        {
            _flood.Color = ColorF.Black;
            _crop.SetInputEffect(0, _flood);
            _crop.Rectangle = new Vector4(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight);
            _convolveMatrix.SetInput(0, bitmap);
            _convolveMatrix.KernelSizeX = 3;
            _convolveMatrix.KernelSizeY = 3;
            _convolveMatrix.KernelMatrix = new float[]
            {
                 0.0f, -1.0f,  0.0f,
                -1.0f,  4.0f, -1.0f,
                 0.0f, -1.0f,  0.0f
            };
            _convolveMatrix.Divisor = 1f;
            _composite.SetInputEffect(0, _crop);
            _composite.SetInputEffect(1, _convolveMatrix);
            return _composite;
        }

        D2D.Effect ApplySepia(D2D.Bitmap1 bitmap)
        {
            _colorMatrix.SetInput(0, bitmap);
            _colorMatrix.Matrix = new Matrix5x4(
                0.393f, 0.349f, 0.272f, 0,
                0.769f, 0.686f, 0.534f, 0,
                0.189f, 0.168f, 0.131f, 0,
                0, 0, 0, 1,
                0, 0, 0, 0
                );
            return _colorMatrix;
        }

        void ExportToGrayscale()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Jpeg Files (*.jpg)|*.jpg";
            sfd.CheckPathExists = true;

            // the user should pick the output file
            if (sfd.ShowDialog().Value)
            {
                // the render target object
                var rt = _d2dContext;

                // create the target Direct2D bitmap for the given DXGI.Surface
                var bpTarget = new D2D.BitmapProperties1(
                    new D2D.PixelFormat(DXGI.Format.B8G8R8A8_UNorm, D2D.AlphaMode.Premultiplied),
                    (float)_bitmap.DpiX, (float)_bitmap.DpiY, D2D.BitmapOptions.Target | D2D.BitmapOptions.CannotDraw);
                Size2L bmpSize = new Size2L(_bitmap.PixelWidth, _bitmap.PixelHeight);
                var targetBmp = D2D.Bitmap1.Create(rt, bmpSize, bpTarget);

                // associate the target bitmap with render target
                rt.SetTarget(targetBmp);

                // start drawing
                rt.BeginDraw();

                // clear the target bitmap
                rt.Clear(null);

                // convert C1Bitmap image to Direct2D image
                var d2dBitmap = _bitmap.ToD2DBitmap1(rt, D2D.BitmapOptions.None);

                // create the Grayscale effect
                _colorMatrix.SetInput(0, d2dBitmap);
                _colorMatrix.Matrix = new Matrix5x4(
                    0.299f, 0.299f, 0.299f, 0,
                    0.587f, 0.587f, 0.587f, 0,
                    0.114f, 0.114f, 0.114f, 0,
                    0, 0, 0, 1,
                    0, 0, 0, 0
                    );

                // and draw the result
                rt.DrawImage(_colorMatrix, Point2F.Empty);
                d2dBitmap.Dispose();

                // now let's draw the text label with shadow
                rt.Transform = Matrix3x2.Rotation(-90f) * Matrix3x2.Translation(6f, 344f);
                _brush.SetColor(ColorF.White);
                rt.DrawTextLayout(new Point2F(-1f, -1f), _textLayout, _brush);
                _brush.SetColor(ColorF.DimGray);
                rt.DrawTextLayout(Point2F.Empty, _textLayout, _brush);
                rt.Transform = Matrix3x2.Identity;

                // finish drawing (all drawing commands are executed now)
                rt.EndDraw();

                // detach the target bitmap
                rt.SetTarget(null);

                // create a temporary C1Bitmap object
                var exportBitmap = new C1Bitmap(_bitmap.ImagingFactory);

                // import the image from Direct2D target bitmap to C1Bitmap
                var srcRect = new RectL(_bitmap.PixelWidth, _bitmap.PixelHeight);
                exportBitmap.Import(targetBmp, rt, srcRect);
                targetBmp.Dispose();

                // save the image to file
                exportBitmap.SaveAsJpeg(sfd.FileName, null);
                exportBitmap.Dispose();
            }
        }
    }
}
