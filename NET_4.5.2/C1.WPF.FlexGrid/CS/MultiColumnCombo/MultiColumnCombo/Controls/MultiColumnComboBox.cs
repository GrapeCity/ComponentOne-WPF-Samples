using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using C1.WPF.FlexGrid;

namespace MultiColumnCombo
{
    /// <summary>
    /// ComboBox that implements the <see cref="MultiColumnComboBox.IsEditable"/> property.
    /// </summary>
    public class MultiColumnComboBox : TextBox
    {
        //-----------------------------------------------------------------
        #region ** fields

        Border _btn;
        DropDownProvider _ddp;

        static Brush _black = new SolidColorBrush(Colors.Black);
        static Brush _brOpaque = new SolidColorBrush(Color.FromArgb(0x80, 0, 0, 0));
        static Brush _brTransparent = new SolidColorBrush(Colors.Transparent);
        static Thickness _thkEmpty = new Thickness(0);
        static Thickness _thkPadding = new Thickness(4, 0, 6, 0);
        const double OFF = 0.3;
        const double ON = 0.8;

        #endregion

        //-----------------------------------------------------------------
        #region ** ctor

        /// <summary>
        /// Initializes a new instance of a <see cref="MultiColumnComboBox"/>.
        /// </summary>
        public MultiColumnComboBox()
        {
            _ddp = new DropDownProvider(this);
            VerticalAlignment = VerticalAlignment.Center;
            Background = _brTransparent;
            BorderThickness = _thkEmpty;
            Padding = _thkPadding;
            MaxDropDownHeight = 300;
        }

        #endregion

        //-----------------------------------------------------------------
        #region ** object model

        /// <summary>
        /// Gets or sets the list of items to show in the dropdown list.
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty = Register("ItemsSource", typeof(IEnumerable), null);
        /// <summary>
        /// Gets or sets the index of the item that is currently selected on the list.
        /// </summary>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }
        public static readonly DependencyProperty SelectedIndexProperty = Register("SelectedIndex", typeof(int), -1);
        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        public object SelectedValue
        {
            get { return GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }
        public static readonly DependencyProperty SelectedValueProperty = Register("SelectedValue", typeof(object), null);

        /// <summary>
        /// Gets or sets the name of the property that contains the control Value.
        /// </summary>
        public string SelectedValuePath
        {
            get { return (string)GetValue(SelectedValuePathProperty); }
            set { SetValue(SelectedValuePathProperty, value); }
        }
        public static readonly DependencyProperty SelectedValuePathProperty = Register("SelectedValuePath", typeof(string), null);
        /// <summary>
        /// Gets or sets the name of the property that is displayed in the control.
        /// </summary>
        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }
        public static readonly DependencyProperty DisplayMemberPathProperty = Register("DisplayMemberPath", typeof(string), null);
        /// <summary>
        /// Gets or sets the <see cref="C1FlexGrid"/> shown in the drop down area of the control.
        /// </summary>
        public C1FlexGrid DropDownGrid
        {
            get { return (C1FlexGrid)GetValue(DropDownGridProperty); }
            set { SetValue(DropDownGridProperty, value); }
        }
        public static readonly DependencyProperty DropDownGridProperty = Register("DropDownGrid", typeof(C1FlexGrid), null);
        /// <summary>
        /// Indicates whether the dropdown is currently being displayed.
        /// </summary>
        public bool IsDroppedDown
        {
            get { return _ddp.IsDroppedDown; }
            set { _ddp.IsDroppedDown = value; }
        }
        /// <summary>
        /// Occurs when the value of the <see cref="IsDroppedDown"/> property changes.
        /// </summary>
        public event EventHandler IsDroppedDownChanged;
        /// <summary>
        /// Raises the <see cref="IsDroppedDownChanged"/> event.
        /// </summary>
        protected internal virtual void OnIsDroppedDownChanged(EventArgs e)
        {
            if (IsDroppedDownChanged != null)
                IsDroppedDownChanged(this, e);
        }
        /// <summary>
        /// Gets or sets the maximum height of the dropdown in pixels.
        /// </summary>
        public double MaxDropDownHeight
        {
            get { return _ddp.MaxDropDownHeight; }
            set { _ddp.MaxDropDownHeight = value; }
        }
        /// <summary>
        /// Gets or sets a value that determines whether the textbox is
        /// editable or restricted to the items in the <see cref="ItemsSource"/> 
        /// collection.
        /// </summary>
        public bool IsEditable
        {
            get { return _ddp.IsEditable; }
            set { _ddp.IsEditable = value; }
        }

        #endregion

        //-----------------------------------------------------------------
        #region ** overrides

        /// <summary>
        /// Overridden to remove focus/mouse borders and add dropdown button
        /// to regular <see cref="TextBox"/> template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            // remove focus, MousOover elements
            foreach (string s in "FocusVisualElement,MouseOverBorder".Split(','))
            {
                var bdr = GetTemplateChild(s) as Border;
                if (bdr != null)
                {
                    bdr.BorderThickness = new Thickness(0);
                }
            }

            // create dropdown button
            _btn = CreateDropDownButton();
            _btn.Visibility = Visibility.Collapsed;

            // left button toggles the drop-down list
            // note: use mouse down and up to work as grid editor
            var gotDownEvent = false;
            _btn.MouseLeftButtonDown += (s, e) =>
                {
                    e.Handled = true;
                    gotDownEvent = true;
                    this.Focus();
                    IsDroppedDown = !IsDroppedDown;
                };
            _btn.MouseLeftButtonUp += (s, e) =>
                {
                    if (!gotDownEvent)
                    {
                        e.Handled = true;
                        this.Focus();
                        IsDroppedDown = !IsDroppedDown;
                    }
                    gotDownEvent = false;
                };
#if SILVERLIGHT
            // change Silverlight template to add column for the dropdown button
            var ctlBdr = GetTemplateChild("Border") as Border;
            var grid = ctlBdr.Child as Grid;
            var cd = grid.ColumnDefinitions;
            if (cd.Count == 0)
            {
                cd.Add(new ColumnDefinition());
            }
            var col = new ColumnDefinition();
            cd.Add(col);
            col.Width = GridLength.Auto;

            // add button to the control
            grid.Children.Add(_btn);
            _btn.SetValue(Grid.ColumnProperty, grid.ColumnDefinitions.Count - 1);
#else
            // change WPF template to add column for the dropdown button
            var grid = new Grid();
            var cd = grid.ColumnDefinitions;
            cd.Add(new ColumnDefinition());
            cd.Add(new ColumnDefinition());
            cd[1].Width = new GridLength(1, GridUnitType.Auto);

            var sv = GetTemplateChild("PART_ContentHost") as ScrollViewer;
            var decorator = sv.Parent as Decorator;
            if (decorator != null)
            {
                var child = decorator.Child;
                decorator.Child = grid;
                grid.Children.Add(child);
                grid.Children.Add(_btn);
                _btn.SetValue(Grid.ColumnProperty, 1);
            }
#endif
            // done
            UpdateButtonVisibility();
            base.OnApplyTemplate();
        }

        #endregion

        //-----------------------------------------------------------------
        #region ** implementation

        internal static Border CreateDropDownButton()
        {
            var p = CreatePolygon(_black, 0, 0, 7, 0, 3.5, 4);
            p.VerticalAlignment = VerticalAlignment.Center;
            p.Margin = _thkPadding;
            p.Opacity = OFF;

            var btn = new Border();
            btn.VerticalAlignment = VerticalAlignment.Stretch;
            btn.Background = new SolidColorBrush(Colors.Transparent);
            btn.Child = p;

            btn.MouseEnter += (s, e) => { p.Opacity = ON; };
            btn.MouseLeave += (s, e) => { p.Opacity = OFF; };

            return btn;
        }
        static Polygon CreatePolygon(Brush brush, params double[] values)
        {
            var p = new Polygon();
            p.VerticalAlignment = VerticalAlignment.Center;
            p.Fill = brush != null ? brush : _brOpaque;
            p.Points = new PointCollection();
            for (int i = 0; i < values.Length - 1; i += 2)
            {
                p.Points.Add(new Point(values[i], values[i + 1]));
            }
            return p;
        }
        void UpdateButtonVisibility()
        {
            if (_btn != null)
            {
                if (ItemsSource != null)
                {
                    _btn.Visibility = Visibility.Visible;
                    if (this.Parent is Border)
                    {
                        this.HorizontalAlignment = HorizontalAlignment.Stretch;
                    }
                }
                else
                {
                    _btn.Visibility = Visibility.Collapsed;
                }
            }
        }

        // dependency property stuff
        static DependencyProperty Register(string name, Type type, object defaultValue)
        {
            var meta = new PropertyMetadata(defaultValue, OnComboBoxPropertyChanged);
            return DependencyProperty.Register(name, type, typeof(MultiColumnComboBox), meta);
        }
        static void OnComboBoxPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cb = (MultiColumnComboBox)d;
            cb.OnComboBoxPropertyChanged(e);
        }
        void OnComboBoxPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            var p = e.Property;
            if (p == ItemsSourceProperty)
            {
                _ddp.DropDownGrid.ItemsSource = this.ItemsSource;
                UpdateButtonVisibility();
            }
            else if (p == SelectedIndexProperty)
            {
                _ddp.SetSelectedIndex(SelectedIndex);
            }
            else if (p == SelectedValueProperty)
            {
                _ddp.SetSelectedValue(SelectedValue);
            }
        }

        #endregion
    }
}
