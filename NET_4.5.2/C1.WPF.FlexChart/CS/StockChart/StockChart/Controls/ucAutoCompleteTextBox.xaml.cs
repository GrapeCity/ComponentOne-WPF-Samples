using C1.WPF;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StockChart.Controls
{
    /// <summary>
    /// Interaction logic for ucAutoCompleteTextBox.xaml
    /// </summary>
    public partial class ucAutoCompleteTextBox : UserControl
    {
        private bool _updating = false;
        private int _maxItems = 9;
        //private Dictionary<string, string> _items;

        private string _selectedText;

        public ucAutoCompleteTextBox()
        {
            InitializeComponent();

            //this.Loaded += async delegate
            //{
            //    await LoadSymbols();
            //};

            //this.DropDownOpensUp = false;

            //if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            //{
            //    this.txt.Loaded += delegate
            //    {
            //        var newBinding = new Binding();
            //        newBinding.Path = new PropertyPath("Text");
            //        newBinding.Source = this.txtSymbol;
            //        newBinding.Mode = BindingMode.TwoWay;
            //        newBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //        this.txt.SetBinding(TextBox.TextProperty, newBinding);

            //        this.txt.Focus(FocusState.Programmatic);
            //        this.txt.SelectionStart = this.txt.Text.Length;
            //    };
            //}
        }
        
        public Dictionary<string, string> ItemsSource
        {
            get { return (Dictionary<string, string>)this.GetValue(ItemsSourceProperty); }
            set { this.SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                "ItemsSource", typeof(Dictionary<string, string>), typeof(ucAutoCompleteTextBox),
                new PropertyMetadata(null)
                );
        public string Placeholder
        {
            get { return (string)this.GetValue(PlaceholderProperty); }
            set { this.SetValue(PlaceholderProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(
                "Placeholder", typeof(string), typeof(ucAutoCompleteTextBox),
                new PropertyMetadata(null)
                );


        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text", typeof(string), typeof(ucAutoCompleteTextBox),
                new PropertyMetadata(null)
                );
        public string Symbol
        {
            get { return (string)this.GetValue(SymbolProperty); }
            set { this.SetValue(SymbolProperty, value); }
        }

        public static readonly DependencyProperty SymbolProperty =
            DependencyProperty.Register(
                "Symbol", typeof(string), typeof(ucAutoCompleteTextBox),
                new PropertyMetadata(null,
                    new PropertyChangedCallback((o,e)=>
                    {
                        ucAutoCompleteTextBox txt = o as ucAutoCompleteTextBox;
                        txt.txtSymbol.Text = e.NewValue.ToString();
                    }))
                );

        public bool DropDownOpensUp
        {
            get
            {
                return (searchSymbols.DropDownDirection == DropDownDirection.ForceAbove);
            }
            set
            {
                if (value)
                {
                    searchSymbols.DropDownDirection = DropDownDirection.ForceAbove;
                }
                else
                {
                    searchSymbols.DropDownDirection = DropDownDirection.ForceBelow;
                }
            }
        }
        

        public event EventHandler SelectionChange;


        private ContentControl _selectedItem = null;
        private ContentControl SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if (_selectedItem != null)
                {
                    (_selectedItem.Content as StackPanel).Background = new SolidColorBrush(Colors.Transparent);
                }
                _selectedItem = value;
                if (value != null)
                {
                    (value.Content as StackPanel).Background = new SolidColorBrush(Color.FromArgb(0xff, 0xb8, 0xd1, 0xe4));
                }
            }
        }


        private void txtSymbol_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((!_updating) && (_selectedText != txtSymbol.Text))
            {
                string text = txtSymbol.Text;
                List<string> items = BuildList(text);
                if (items.Count == 0)
                {
                    //if (!Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
                    //    searchSymbols.IsDropDownOpen = false;
                    //else
                        listBox.Items.Clear();
                }
                else
                {
                    listBox.Items.Clear();
                    foreach (string key in items)
                    {
                        listBox.Items.Add(BuildItem(text, key, ItemsSource[key]));
                    }
                    searchSymbols.IsDropDownOpen = true;
                }
            }
        }

        private void txtSymbol_KeyDown(object sender, KeyEventArgs e)
        {
            int delta = 0;
            switch (e.Key)
            {
                case Key.Down:
                    delta = 1;
                    break;

                case Key.Up:
                    delta = -1;
                    break;

                case Key.Enter:
                    CommitListSelection();
                    break;

                case Key.Escape:
                    searchSymbols.IsDropDownOpen = false;
                    break;
            }

            int oldIndex;
            if ((_selectedItem != null) && listBox.Items.Contains(_selectedItem))
            {
                oldIndex = listBox.Items.IndexOf(_selectedItem);
            }
            else
            {
                oldIndex = (DropDownOpensUp ? listBox.Items.Count : -1);
            }
            int index = Math.Min(Math.Max(0, oldIndex + delta), listBox.Items.Count - 1);
            if ((index >= 0) && (index < listBox.Items.Count))
            {
                SelectedItem = listBox.Items[index] as ContentControl;
            }
        }

        private void txtSymbol_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (!Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            //{
                searchSymbols.IsDropDownOpen = false;
            //}
        }




        // methods used for building the pick list
        private List<string> BuildList(string text)
        {
            List<string> list = new List<string>();
            if (text.Length > 0)
            {
                // add matches on symbol first
                foreach (string key in ItemsSource.Keys)
                {
                    if (key.StartsWith(text, StringComparison.CurrentCultureIgnoreCase))
                    {
                        list.Add(key);
                        if (list.Count >= _maxItems)
                            break;
                    }
                }

                // add matches on name second
                foreach (string key in ItemsSource.Keys)
                {
                    if (ItemsSource[key].IndexOf(text, StringComparison.CurrentCultureIgnoreCase) > -1 &&
                        !list.Contains(key))
                    {
                        list.Add(key);
                        if (list.Count >= _maxItems)
                            break;
                    }
                }
            }
            return list;
        }



        private FrameworkElement BuildItem(string search, string key, string text)
        {
            StackPanel p = new StackPanel();
            p.Orientation = Orientation.Horizontal;
            p.Background = new SolidColorBrush(Colors.Transparent);
            p.Children.Add(BuildSubItem(search, key, 60));
            p.Children.Add(BuildSubItem(search, text, 200));

            ContentControl c = new ContentControl();
            c.MouseLeftButtonDown += C_MouseLeftButtonDown;
            c.Content = p;

            return c;
        }

        private void C_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectedItem = sender as ContentControl;
            if (SelectedItem != null)
            {
                CommitListSelection();
            }
        }
        private void CommitListSelection()
        {
            if (_selectedItem == null)
            {
                return;
            }
            StackPanel sp = (StackPanel)(_selectedItem).Content;
            if (sp != null)
            {
                // apply text to textbox, hide popup
                _updating = true;
                //_symbol = (sp.Children[0] as TextBlock).Text;
                //_text = (sp.Children[1] as TextBlock).Text;
                Symbol = GetTextBlockText(sp.Children[0] as TextBlock);
                Text = GetTextBlockText(sp.Children[1] as TextBlock);

                txtSymbol.Text = Symbol;
                _updating = false;

                // fire event to indicate a choice was made
                CommitSelection();

                searchSymbols.IsDropDownOpen = false;

                _selectedText = txtSymbol.Text;
                txtSymbol.Select(0, txtSymbol.Text.Length);
            }
        }

        private string GetTextBlockText(TextBlock tb)
        {
            StringBuilder s = new StringBuilder();
            foreach (var line in tb.Inlines)
            {
                if (line is LineBreak)
                    s.AppendLine();
                else if (line is Run)
                    s.Append(((Run)line).Text);
            }
            return s.ToString();
        }

        // commit C1TextBox change
        private void CommitSelection()
        {
            if (SelectionChange != null)
            {
                SelectionChange(this, new EventArgs());
            }
        }

        private TextBlock BuildSubItem(string search, string text, double width)
        {
            TextBlock tb = new TextBlock();
            tb.Text = string.Empty;
            tb.Margin = new Thickness(5);

            for (int start = 0; ;)
            {
                // look for a match
                int index = text.IndexOf(search, start, StringComparison.CurrentCultureIgnoreCase);

                // match not found, add remainder and be done
                if (index < 0)
                {
                    CreateRun(tb, false, text.Substring(start));
                    break;
                }

                // match found: add regular and bold parts
                if (index > start)
                {
                    CreateRun(tb, false, text.Substring(start, index - start));
                }
                CreateRun(tb, true, text.Substring(index, search.Length));
                start = index + search.Length;
            }

            tb.Width = width;
            return tb;
        }
        private void CreateRun(TextBlock tb, bool highlight, string text)
        {
            Run run = new Run();
            run.Text = text;
            run.FontFamily = FontFamily;
            run.FontSize = FontSize;
            run.FontWeight = highlight ? FontWeights.Bold : FontWeights.Normal;

            tb.Inlines.Add(run);
        }

    }
}
