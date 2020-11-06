using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace TileSamplesLib
{
    /// <summary>
    /// Interaction logic for TileTypes.xaml
    /// </summary>
    public partial class TileTypes : UserControl
    {
        // the sample collection to use as ContentSource
        public ObservableCollection<object> CustomObjects
        {
            get;
            set;
        }
        public TileTypes()
        {
            CustomObjects = new ObservableCollection<object>();
            CustomObjects.Add(new CustomObject() { Header = "object 1", Background= new SolidColorBrush(Color.FromArgb(0xFF, 0x88, 0xbd, 0xe6)) });
            CustomObjects.Add(new CustomObject() { Header = "object 2", Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xfb, 0xB2, 0x58)) });
            CustomObjects.Add(new CustomObject() { Header = "object 3", Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xf6, 0xaa, 0xc9)) });
            CustomObjects.Add(new CustomObject() { Header = "object 4", Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xbf, 0xa5, 0x54)) });
            CustomObjects.Add(new CustomObject() { Header = "object 5", Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xbc, 0x99, 0xc7)) });
            CustomObjects.Add(new CustomObject() { Header = "object 6", Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x8c, 0x8c, 0x8c)) });

            InitializeComponent();
        }
    }

    public class CustomObject
    {
        public string Header
        {
            get;
            set;
        }
        public Brush Background
        {
            get;
            set;
        }
    }
}
