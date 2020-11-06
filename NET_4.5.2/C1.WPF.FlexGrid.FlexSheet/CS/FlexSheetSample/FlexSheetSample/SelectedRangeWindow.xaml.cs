using C1.WPF.FlexGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using C1.WPF;
using System.Globalization;
using System.Reflection;

namespace FlexSheetSample
{
    /// <summary>
    /// Interaction logic for SelectedRangeWindow.xaml
    /// </summary>
    public partial class SelectedRangeWindow : Window
    {
        #region ** fields

        C1FlexSheet _owner;
        CellRange _textRange = new CellRange(-1, -1);
        CellRange _locationRange = new CellRange(-1, -1);

        private const double MINHEIGHT = 200;
        private const double MINWIDTH = 330;

        Image _textRangeUpImg, _textRangeDownImg, _locationUpImg, _locationDownImg;

        #endregion

        public SelectedRangeWindow()
        {
            InitializeComponent();
            var selectionUpImageResource = Application.GetResourceStream(new Uri("/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component/Resources/SelectionUp.png", UriKind.Relative));
           

            var selectionUpImage = new BitmapImage();
            selectionUpImage.BeginInit();
            selectionUpImage.StreamSource = selectionUpImageResource.Stream;
            selectionUpImage.EndInit();


            var selectionDownImageResource = Application.GetResourceStream(new Uri("/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component/Resources/SelectionDown.png", UriKind.Relative));
           
            var selectionDownImage = new BitmapImage();
            selectionDownImage.BeginInit();
            selectionDownImage.StreamSource = selectionDownImageResource.Stream;
            selectionDownImage.EndInit();


            _textRangeUpImg = new Image() { Source = selectionUpImage };
            _textRangeDownImg = new Image() { Source = selectionDownImage };
            _btnTextRange.Content = _textRangeUpImg;


            _locationUpImg = new Image() { Source = selectionUpImage.Clone() };
            _locationDownImg = new Image() { Source = selectionDownImage.Clone() };
            _btnLocationRange.Content = _locationUpImg;
        }

        public SelectedRangeWindow(C1FlexSheet owner)
            : this()
        {
            _owner = owner;
            _owner.SelectionChanged += _owner_SelectionChanged;
            _tbTextRange.Text = owner.GetAddress(owner.Selection, true);
            _textRange = owner.Selection;
        }

        #region Event Handler
        void _owner_SelectionChanged(object sender, CellRangeEventArgs e)
        {
            TextBox tb = null;
            if (_tbTextRange.IsFocused)
            {
                tb = _tbTextRange;
                _textRange = _owner.Selection;
            }
            else
            {
                tb = _tbLocationRange;
                _locationRange = _owner.Selection;
            }
            tb.Text = _owner.GetAddress(_owner.Selection, true);
        }

        private void _btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void _btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (_locationRange.IsValid && _textRange.IsValid)
            {
                if (_locationRange.RowSpan == _textRange.RowSpan && _locationRange.ColumnSpan == 1)
                {
                    int origionLocationRow = _locationRange.TopRow;
                    for (int row = _textRange.TopRow; row <= _textRange.BottomRow; row++)
                    {
                        List<double> datas = new List<double>();
                        for (int col = _textRange.LeftColumn; col <= _textRange.RightColumn; col++)
                        {
                            object originValue = _owner[row, col];
                            if (originValue != null && originValue.GetType().IsNumeric())
                            {
                                double value = (double)Convert.ChangeType(originValue, typeof(double), CultureInfo.CurrentCulture);
                                datas.Add(value);
                            }
                            else
                            {
                                datas.Add(0);
                            }
                        }
                        _owner.InsertSparkLine(SparkLineType.Line, datas, _owner.Sheets.SelectedSheet, new CellRange(origionLocationRow, _locationRange.LeftColumn));
                        origionLocationRow++;
                    }
                    Close();
                }
                else if (_locationRange.ColumnSpan == _textRange.ColumnSpan && _locationRange.RowSpan == 1)
                {
                    int origionLocationColumn = _locationRange.LeftColumn;
                    for (int col = _textRange.LeftColumn; col <= _textRange.RightColumn; col++)
                    {
                        List<double> datas = new List<double>();
                        for (int row = _textRange.TopRow; row <= _textRange.BottomRow; row++)
                        {
                            object originValue = _owner[row, col];
                            if (originValue != null && originValue.GetType().IsNumeric())
                            {
                                double value = (double)Convert.ChangeType(originValue, typeof(double), CultureInfo.CurrentCulture);
                                datas.Add(value);
                            }
                            else
                            {
                                datas.Add(0);
                            }
                        }
                        _owner.InsertSparkLine(SparkLineType.Line, datas, _owner.Sheets.SelectedSheet, new CellRange(_locationRange.TopRow, origionLocationColumn));
                        origionLocationColumn++;
                    }
                    Close();
                }
                else
                {
                    MessageBox.Show("Location reference is not valid");
                }
            }
        }

        private void _btnTextRange_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            btn.Content = btn.Content == _textRangeUpImg ? _textRangeDownImg : _textRangeUpImg;
            _tbTextRange.Focus();
            SwitchVisibility(_gridLocationRange);
            SwitchVisibility(_gridResult);
        }

        private void _btnLocationRange_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            btn.Content = btn.Content == _locationUpImg ? _locationDownImg : _locationUpImg;
            _tbLocationRange.Focus();
            SwitchVisibility(_gridDataRange);
            SwitchVisibility(_gridResult);
        }
        #endregion

        private void SwitchVisibility(UIElement element)
        {
            if (element.Visibility == Visibility.Visible)
            {
                this.MinHeight = 0;
                element.Visibility = Visibility.Collapsed;
                this.SizeToContent = System.Windows.SizeToContent.Height;
                this.ResizeMode = System.Windows.ResizeMode.NoResize;
            }
            else
            {
                this.MinHeight = MINHEIGHT;
                element.Visibility = Visibility.Visible;
                this.SizeToContent = System.Windows.SizeToContent.Manual;
                this.ResizeMode = System.Windows.ResizeMode.CanResize;
            }
            this.InvalidateVisual();
        }
    }
}
