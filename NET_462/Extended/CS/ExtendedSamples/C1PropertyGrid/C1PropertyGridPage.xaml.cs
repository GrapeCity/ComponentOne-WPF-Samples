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
using C1.WPF;
using C1.WPF.Extended;
using C1.WPF.Extended.PropertyGrid;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace ExtendedSamples
{
    /// <summary>
    /// Interaction logic for DemoPropertyGrid.xaml
    /// </summary>
    public partial class DemoPropertyGrid : UserControl
    {
         TestClass tc = new TestClass();
         
        public DemoPropertyGrid()
        {
            InitializeComponent();
            propertyGrid.PropertyBoxesLoaded += _propertyGrid_PropertyBoxesLoaded;
            this.targetButton.Click += new RoutedEventHandler(targetButton_Click);
            (this.stackControl1.Children[0] as CheckBox).IsChecked = true;
            this.UpdateCheckContent();
        }

        void targetButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            this.targetButton.Content = "content changed";
        }

        private void CheckBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (e.OriginalSource != sender) return;
            this.UpdateCheckContent();
        }
      
        private void UpdateCheckContent()
        {
            CheckBox[] elements = this.stackControl1.Children.Cast<CheckBox>().Where(x => x.IsChecked.Value).ToArray();
            List<object> listelement = new List<object>();
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i].IsChecked.Value)
                {
                    listelement.Add(elements[i].Content);
                }
            }
            this.propertyGrid.SelectedObject = null;
            if (listelement.Count > 0)
            {
                for (int i = 0; i < listelement.Count; i++)
                {
                    this.propertyGrid.SelectedObjects.Add(listelement[i]);
                }
            }                 
        }
      
        public bool ShowResetButton
        {
            get { return propertyGrid.ShowResetButton; }
            set { propertyGrid.ShowResetButton = value; }
        }

        public bool ShowDescription
        {
            get { return propertyGrid.ShowDescription; }
            set { propertyGrid.ShowDescription = value; }
        }

        public bool AutoGenerateProperties
        {
            get { return propertyGrid.AutoGenerateProperties; }
            set { propertyGrid.AutoGenerateProperties = value; }
        }

        public bool AutoGenerateMethods
        {
            get { return propertyGrid.AutoGenerateMethods; }
            set { propertyGrid.AutoGenerateMethods = value; }
        }

        public Brush DEMO_Background
        {
            get
            {
                return propertyGrid.Background;
            }
            set
            {
                propertyGrid.Background = value;
            }
        }

        public Brush DEMO_Foreground
        {
            get
            {
                return propertyGrid.Foreground;
            }
            set
            {
                propertyGrid.Foreground = value;
            }
        }
        public Brush DEMO_BorderBrush
        {
            get
            {
                return propertyGrid.BorderBrush;
            }
            set
            {
                propertyGrid.BorderBrush = value;
            }
        }

        public Brush MouseOverBrush
        {
            get
            {
                return propertyGrid.MouseOverBrush;
            }
            set
            {
                propertyGrid.MouseOverBrush = value;
            }
        }

        public Brush PressedBrush
        {
            get
            {
                return propertyGrid.PressedBrush;
            }
            set
            {
                propertyGrid.PressedBrush = value;
            }
        }
        public Brush CategoryBackground
        {
            get
            {
                return propertyGrid.CategoryBackground;
            }
            set
            {
                propertyGrid.CategoryBackground = value;
            }
        }
        public Brush CategoryForeground
        {
            get
            {
                return propertyGrid.CategoryForeground;
            }
            set
            {
                propertyGrid.CategoryForeground = value;
            }
        }

        private void _propertyGrid_PropertyBoxesLoaded(object sender, EventArgs e)
        {
        }

        private void _propertyGrid_PropertyBoxAdded(object sender, C1.WPF.Extended.PropertyBoxChangedEventArgs e)
        {
            if (e.PropertyBox.PropertyAttribute.MemberName == "Opacity")
            {
                (e.PropertyBox.CurrentEditor as C1NumericBox).Value = 0;
            }
        }
    }

    public class TestClass
    {
        public TestClass()
        {
            this.Content = "MyContent";

        }
        object _Content;
        [Category("Content")]
        public object Content
        {
            get { return _Content; }
            set { _Content = value;
                if (this.DataChanged != null)
                    this.DataChanged(this, EventArgs.Empty);
            }
        }
        public event EventHandler DataChanged;

        int _ID;
        [Display(Description = "Gets or sets ID of the element.")]
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

          double _height;

          public double Height
          {
              get { return _height; }
              set { _height = value;
              if (this.DataChanged != null)
                  this.DataChanged(this, EventArgs.Empty);
              }
          }
          double _width;

          public double Width
          {
              get { return _width; }
              set { _width = value;
              if (this.DataChanged != null)
                  this.DataChanged(this, EventArgs.Empty);
              }
          }
            
    }
}
