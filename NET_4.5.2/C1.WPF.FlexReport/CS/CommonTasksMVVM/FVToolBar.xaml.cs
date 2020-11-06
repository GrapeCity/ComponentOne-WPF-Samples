using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

using C1.WPF.FlexViewer;

namespace CommonTasksMVVM
{
    /// <summary>
    /// Interaction logic for FVToolBar.xaml
    /// </summary>
    public partial class FVToolBar : UserControl
    {
        static readonly double[] FixedZooms = new double[] { 0.25, 0.5, .75, 1, 1.25, 1.5, 2, 4, 8 };

        C1FlexViewerPane _fvp;
        bool _innerZoomChange;

        public FVToolBar()
        {
            InitializeComponent();
        }

        internal void SetHost(C1FlexViewerPane fvp)
        {
            _fvp = fvp;
            fvp.PropertyChanged += HandlePropertyChanged;
            AssignCommands();
            AssignBindings();
            pageNumberTextBox.KeyDown += PageNumberTextBox_KeyDown; ;
            InitializeZoomPart();
        }

        void PageNumberTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _fvp.PageHistoryTracking = true;
                _fvp.FocusFirstElement();
                _fvp.PageHistoryTracking = false;
                e.Handled = true;
            }
        }

        void AssignCommands()
        {
            printButton.Command = _fvp.PrintCommand;
            refreshButton.Command = _fvp.RefreshCommand;
            stopOperationButton.Command = _fvp.StopOperationCommand;

            firstPageButton.Command = _fvp.FirstPageCommand;
            previousPageButton.Command = _fvp.PreviousPageCommand;
            nextPageButton.Command = _fvp.NextPageCommand;
            lastPageButton.Command = _fvp.LastPageCommand;

            navigateBackwardButton.Command = _fvp.NavigateBackwardCommand;
            navigateForwardButton.Command = _fvp.NavigateForwardCommand;

            menuRotateCW.Command = _fvp.RotateClockwiseCommand;
            menuRotateCCW.Command = _fvp.RotateCounterClockwiseCommand;
            menuOnePage.Command = _fvp.OnePageViewCommand;
            menuFacingPages.Command = _fvp.FacingPagesCommand;
            menuTwoPages.Command = _fvp.TwoPagesViewCommand;
            menuFourPages.Command = _fvp.FourPagesViewCommand;
        }

        void AssignBindings()
        {
            printLayoutButton.SetBinding(ToggleButton.IsCheckedProperty,
                new Binding() { Source = _fvp, Path = new PropertyPath("Paginated"), Mode = BindingMode.TwoWay });
            thumbnailsButton.SetBinding(ToggleButton.IsCheckedProperty,
                new Binding() { Source = _fvp, Path = new PropertyPath("ThumbViewMode"), Mode = BindingMode.TwoWay });
            thumbnailsButton.SetBinding(UIElement.IsEnabledProperty,
                new Binding() { Source = _fvp, Path = new PropertyPath("Paginated") });
            pageNumberTextBox.SetBinding(TextBox.TextProperty,
                new Binding() { Source = _fvp, Path = new PropertyPath("PageNumber"), Mode = BindingMode.TwoWay });
            pageCountPresenter.SetBinding(ContentPresenter.ContentProperty,
                new Binding() { Source = _fvp, Path = new PropertyPath("PageCount") });
            menuLayout.SetBinding(UIElement.IsEnabledProperty,
                new Binding() { Source = _fvp, Path = new PropertyPath("Paginated") });
            stopOperationButton.SetBinding(UIElement.VisibilityProperty,
                new Binding() { Source = _fvp, Path = new PropertyPath("Busy"), Converter = new C1.WPF.VisibilityConverter() });
            selectToolButton.SetBinding(ToggleButton.IsCheckedProperty,
                new Binding() { Source = _fvp, Path = new PropertyPath("SelectMouseMode"), Mode = BindingMode.TwoWay });
            sbText.SetBinding(TextBlock.TextProperty,
                new Binding() { Source = _fvp, Path = new PropertyPath("StatusText") });
        }

        void InitializeZoomPart()
        {
            zoomCombo.IsEditable = true;
            zoomCombo.ItemConverter = new ZoomTypeConverter();
            zoomCombo.KeyDown += ZoomCombo_KeyDown;

            var list = new List<object>();
            list.Add(FlexViewerZoomMode.ActualSize);
            list.Add(FlexViewerZoomMode.PageWidth);
            list.Add(FlexViewerZoomMode.WholePage);
            list.AddRange(FixedZooms.Cast<object>());
            zoomCombo.ItemsSource = list;
            switch (_fvp.ZoomMode)
            {
                case FlexViewerZoomMode.ActualSize:
                    zoomCombo.SelectedIndex = 0;
                    break;
                case FlexViewerZoomMode.PageWidth:
                    zoomCombo.SelectedIndex = 1;
                    break;
                case FlexViewerZoomMode.WholePage:
                    zoomCombo.SelectedIndex = 2;
                    break;
                case FlexViewerZoomMode.Custom:
                    zoomCombo.SelectedItem = (double)_fvp.ZoomFactor;
                    break;
            }
            zoomCombo.SelectionCommitted += ZoomCombo_SelectionCommitted;
        }

        void ZoomCombo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _fvp.FocusFirstElement();
                e.Handled = true;
            }
        }

        void ZoomCombo_SelectionCommitted(object sender, C1.WPF.PropertyChangedEventArgs<object> e)
        {
            if (!_innerZoomChange)
            {
                var value = zoomCombo.SelectedItem;
                if (value is double)
                {
                    _fvp.ZoomFactor = (float)((double)value);
                }
                else if (value is FlexViewerZoomMode)
                {
                    _fvp.ZoomMode = (FlexViewerZoomMode)value;
                }
                _fvp.FocusFirstElement();
            }
        }

        void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ZoomMode":
                    switch (_fvp.ZoomMode)
                    {
                        case FlexViewerZoomMode.ActualSize:
                            _innerZoomChange = true;
                            zoomCombo.SelectedIndex = 0;
                            _innerZoomChange = false;
                            break;
                        case FlexViewerZoomMode.PageWidth:
                            _innerZoomChange = true;
                            zoomCombo.SelectedIndex = 1;
                            _innerZoomChange = false;
                            break;
                        case FlexViewerZoomMode.WholePage:
                            _innerZoomChange = true;
                            zoomCombo.SelectedIndex = 2;
                            _innerZoomChange = false;
                            break;
                    }
                    break;

                case "ZoomFactor":
                    if (_fvp.ZoomMode == FlexViewerZoomMode.Custom)
                    {
                        _innerZoomChange = true;
                        zoomCombo.SelectedItem = (double)_fvp.ZoomFactor;
                        _innerZoomChange = false;
                    }
                    break;
            }
        }
    }

    public class ZoomTypeConverter : TypeConverter
    {
        Dictionary<object, object> _cache = new Dictionary<object, object>();

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value != null)
            {
                var converter = new ZoomConverter();
                var name = converter.Convert(value, destinationType, null, null);
                if (name != null)
                {
                    _cache[name] = value;
                    return name;
                }
            }
            return null;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value != null)
            {
                var str = value.ToString().Trim();

                object zoom;
                if (_cache.TryGetValue(str, out zoom))
                {
                    return zoom;
                }

                if (str.EndsWith("%"))
                {
                    str = str.Substring(0, str.Length - 1);
                }
                int scale;
                if (int.TryParse(str, NumberStyles.None, CultureInfo.InvariantCulture, out scale))
                {
                    return scale / 100.0;
                }
            }
            return 1.0;
        }
    }

    public class ZoomConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                return (int)((double)value * 100) + "%";
            }
            if (value is FlexViewerZoomMode)
            {
                var mode = (FlexViewerZoomMode)value;
                switch (mode)
                {
                    case FlexViewerZoomMode.ActualSize:
                        return FVToolBarStrings.ZoomMode_ActualSize;

                    case FlexViewerZoomMode.PageWidth:
                        return FVToolBarStrings.ZoomMode_PageWidth;

                    case FlexViewerZoomMode.WholePage:
                        return FVToolBarStrings.ZoomMode_WholePage;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class FVToolBarStrings
    {
        public static string FacingPagesMenuHeader
        {
            get { return "Facing Pages"; }
        }

        public static string FirstPageTooltip
        {
            get { return "First Page"; }
        }

        public static string FourPagesMenuHeader
        {
            get { return "Four Pages"; }
        }

        public static string LastPageTooltip
        {
            get { return "Last Page"; }
        }

        public static string LayoutMenuHeader
        {
            get { return "Layout"; }
        }

        public static string LayoutMenuTooltip
        {
            get { return "Layout Options"; }
        }

        public static string NavigateBackwardTooltip
        {
            get { return "Navigate Backward"; }
        }

        public static string NavigateForwardTooltip
        {
            get { return "Navigate Forward"; }
        }

        public static string NextPageTooltip
        {
            get { return "Next Page"; }
        }

        public static string OnePageMenuHeader
        {
            get { return "One Page"; }
        }

        public static string PageCountTooltip
        {
            get { return "Page Count"; }
        }

        public static string PageLabel
        {
            get { return "Page"; }
        }

        public static string PageNumberTooltip
        {
            get { return "Page Number"; }
        }

        public static string PreviousPageTooltip
        {
            get { return "Previous Page"; }
        }

        public static string PrintLayoutTooltip
        {
            get { return "Print Layout"; }
        }

        public static string PrintTooltip
        {
            get { return "Print"; }
        }

        public static string RefreshTooltip
        {
            get { return "Refresh"; }
        }

        public static string RotateCCWMenuHeader
        {
            get { return "Rotate Counter Clockwise"; }
        }

        public static string RotateCWMenuHeader
        {
            get { return "Rotate Clockwise"; }
        }

        public static string SaveTooltip
        {
            get { return "Save"; }
        }

        public static string SelectToolTooltip
        {
            get { return "Text Selection Tool"; }
        }

        public static string StopOperationTooltip
        {
            get { return "Stop the Operation"; }
        }

        public static string ThumbnailsTooltip
        {
            get { return "Page Thumbnails"; }
        }

        public static string TwoPagesMenuHeader
        {
            get { return "Two Pages"; }
        }

        public static string ZoomComboTooltip
        {
            get { return "Zoom Options"; }
        }

        public static string ZoomMode_ActualSize
        {
            get { return "Actual Size"; }
        }

        public static string ZoomMode_PageWidth
        {
            get { return "Page Width"; }
        }

        public static string ZoomMode_WholePage
        {
            get { return "Whole Page"; }
        }
    }
}
