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
using C1.WPF.Core;
using C1.Util.DX.Direct2D.Effects;

namespace BitmapExplorer
{
    public partial class TransformDemo : UserControl
    {
        C1Bitmap _bitmap;
        C1Bitmap _savedCopy;
        Point _start;
        C1DragHelper _dragHelper;
        Rect _selection;
        Window _window;
        bool _initialized;
        //calculator of the cumulative Scale Factor
        float _scaleFactor = 1.0f;

        public TransformDemo()
        {
            InitializeComponent();
            Tag = Properties.Resources.TransformDemoDesc;
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
            Assembly asm = typeof(TransformDemo).Assembly;
            using (Stream stream = asm.GetManifestResourceStream("BitmapExplorer.Resources.HousePlan.jpg"))
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
                _scaleFactor = 1.0f;  
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
            var newBitmap = _bitmap.Transform(t);
            _bitmap.Dispose();
            _bitmap = newBitmap;
            UpdateImage();
        }
        void Crop_Clicked(object sender, RoutedEventArgs e)
        {
            var cropRect = ((RectD)_selection).Round();
            //this means nothing is selected and we have to do nothing
            // image stays emplaced 
            if (cropRect.X == 0 && cropRect.Y == 0 && 
                cropRect.Width == _bitmap.PixelWidth && cropRect.Height== _bitmap.PixelHeight) return;
            if ((int)(cropRect.Width / _scaleFactor) <= 0 && (int)(cropRect.Height / _scaleFactor) <= 0) 
            {
                MessageBox.Show("The selected area is too small. Please select a larger area and try again.",
                    "Selection Too Small", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            };
            // it should be greater than 1.0 as
            // when we have scale in and out the cumulative scale factor will still be 1
            if (_scaleFactor>1.0f)
            {
                cropRect = AdjustSelectionToScale(cropRect);
            }
           //perform changes in the image
            ApplyTransform(new Clipper(new ImageRect(cropRect)));         
            //reverse the value as after zoom the image returns to its original size
            // user can perform immediately again cutting the image 
            _scaleFactor = 1.0f;
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
            if (_savedCopy == null) return;

            var tempBitmap = _savedCopy.Transform();
            int newWidth = (int)(tempBitmap.PixelWidth * _scaleFactor * 1.6f + 0.5f);
            int newHeight = (int)(tempBitmap.PixelHeight * _scaleFactor * 1.6f + 0.5f);

            if (newWidth * newHeight > 100000000)
            {
                tempBitmap.Dispose();
                return;
            }

            var scaledBitmap = tempBitmap.Transform(new Scaler(newWidth, newHeight, InterpolationMode.HighQualityCubic));
            tempBitmap.Dispose();

            _bitmap.Dispose();
            _bitmap = scaledBitmap;
            UpdateImage();

            _scaleFactor *= 1.6f;
        }

        void ScaleOut_Clicked(object sender, RoutedEventArgs e)
        {
            if (_savedCopy == null) return;

            var tempBitmap = _savedCopy.Transform();
            int newWidth = (int)(tempBitmap.PixelWidth * _scaleFactor * 0.625f + 0.5f);
            int newHeight = (int)(tempBitmap.PixelHeight * _scaleFactor * 0.625f + 0.5f);

            if (newWidth <= 16 || newHeight <= 16)
            {
                tempBitmap.Dispose();
                return;
            }

            var scaledBitmap = tempBitmap.Transform(new Scaler(newWidth, newHeight, InterpolationMode.HighQualityCubic));
            tempBitmap.Dispose();

            _bitmap.Dispose();
            _bitmap = scaledBitmap;
            UpdateImage();

            _scaleFactor *= 0.625f;
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
            bottomMask.Height = Math.Max(0,_bitmap.PixelHeight - _selection.Bottom);
            leftMask.Width = _selection.Left;
            rightMask.Width = Math.Max(0,_bitmap.PixelWidth - _selection.Right);
        }

        // this method is used to Adjust the Selection coordinates according to zoomed scale
        RectL AdjustSelectionToScale(RectL userInput)
        {
           // Calculate the user  selection and adjust according to scale
            int zoomedX = (int)(userInput.X / _scaleFactor);
            int zoomedY = (int)(userInput.Y / _scaleFactor);
            int zoomedCropWidth = (int)(userInput.Width / _scaleFactor);
            int zoomedCropHeight = (int)(userInput.Height / _scaleFactor);

        
           // adjusted rectangle with the new coordinates 
            RectL clipRect = new RectD(
                zoomedX,
                zoomedY,
                zoomedCropWidth,
                zoomedCropHeight
            ).Round();


            // Ensure the calculated rectangle is within the bounds of the original bitmap
            if (clipRect.Right > _bitmap.PixelWidth)
            {
                clipRect.Width = _bitmap.PixelWidth - clipRect.X;
            }
            if (clipRect.Bottom > _bitmap.PixelHeight)
            {
                clipRect.Height = _bitmap.PixelHeight - clipRect.Y;
            }
            if (clipRect.X == 0)
            {
                clipRect.X =userInput.X;
            }
            if (clipRect.Y ==0)
            {
                clipRect.Y = userInput.Y;
            }          
            
            return clipRect;
        }

    }
}
