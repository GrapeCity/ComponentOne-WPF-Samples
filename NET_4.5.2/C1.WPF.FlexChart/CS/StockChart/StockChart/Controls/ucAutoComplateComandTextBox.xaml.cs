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

namespace StockChart
{
    /// <summary>
    /// Interaction logic for ucAutoComplateComandTextBox.xaml
    /// </summary>
    public partial class ucAutoComplateComandTextBox : UserControl
    {
        public ucAutoComplateComandTextBox()
        {
            InitializeComponent();
            //this.btnLoad.Command = cmdLoad;
            //this.btnLoad.CommandTarget = this.txtSymbol;
            //CommandBinding cb = new CommandBinding();
            //cb.Command = cmdLoad;
            //cb.CanExecute += (o ,e)=>
            //{
            //    if (!string.IsNullOrEmpty(this.txtSymbol.Text))
            //    {
            //        if (ItemsSource.Keys.Contains(this.txtSymbol.Text))
            //        {
            //            e.CanExecute = true;
            //            return;
            //        }
            //    }
            //    e.CanExecute = false;
            //};
            //cb.Executed += (o, e) =>
            //{
            //    var text = this.txtSymbol.Text;

            //};
            //this.CommandBindings.Add(cb);
        }
        
        public object Placeholder
        {
            get { return this.GetValue(PlaceholderProperty); }
            set { this.SetValue(PlaceholderProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(
                "Placeholder", typeof(object), typeof(ucAutoComplateComandTextBox),
                new PropertyMetadata(null)
                );

        public string Text
        {
            get { return this.GetValue(TextProperty).ToString(); }
            set { this.SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text", typeof(string), typeof(ucAutoComplateComandTextBox),
                new PropertyMetadata(null)
                );

        public Dictionary<string, string> ItemsSource
        {
            get {return (Dictionary<string, string>)this.GetValue(ItemsSourceProperty); }
            set { this.SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                "ItemsSource", typeof(Dictionary<string, string>), typeof(ucAutoComplateComandTextBox),
                new PropertyMetadata(null)
                );

        public object ButtonContent
        {
            get { return this.GetValue(ButtonContentProperty); }
            set { this.SetValue(ButtonContentProperty, value); }
        }

        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.Register(
                "ButtonContent", typeof(object), typeof(ucAutoComplateComandTextBox),
                new PropertyMetadata(null)
                );
        
        public string SymbolCode
        {
            get { return this.GetValue(SymbolCodeProperty).ToString(); }
            set { this.SetValue(SymbolCodeProperty, value); }
        }

        public static readonly DependencyProperty SymbolCodeProperty =
            DependencyProperty.Register(
                "SymbolCode", typeof(string), typeof(ucAutoComplateComandTextBox),
                new PropertyMetadata(null)
                );
        
        public ICommand Command1
        {
            get { return (ICommand)this.GetValue(Command1Property); }
            set { this.SetValue(Command1Property, value); }
        }

        public static readonly DependencyProperty Command1Property =
            DependencyProperty.Register(
                "Command1", typeof(ICommand), typeof(ucAutoComplateComandTextBox),
                new PropertyMetadata(null, (o, e)=>
                {

                })
                );


        //static AutoCompleteFilterPredicate<object> _SymbolFilter = (text, obj) =>
        //        !string.IsNullOrEmpty(text) && text.Length >= 2 &&
        //(
        //        ((KeyValuePair<string, string>)obj).Key.ToUpper().Contains(text.ToUpper()) ||
        //        ((KeyValuePair<string, string>)obj).Value.ToUpper().Contains(text.ToUpper())
        //);
        //public AutoCompleteFilterPredicate<object> SymbolFilter
        //{
        //    get
        //    {
        //        return _SymbolFilter;
        //    }
        //}

    }

    
}
