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

namespace BasicControls
{
    /// <summary>
    /// Interaction logic for DemoHyperPanel.xaml
    /// </summary>
    public partial class DemoHyperPanel : UserControl
    {
        public DemoHyperPanel()
        {
            InitializeComponent();
        }

        #region Object model

        public Orientation Orientation
        {
            get
            {
                return hyperPanel.Orientation;
            }
            set
            {
                hyperPanel.Orientation = value;
            }
        }

        public double Distribution
        {
            get
            {
                return hyperPanel.Distribution;
            }
            set
            {
                hyperPanel.Distribution = value;
            }
        }

        public bool ApplyOpacity
        {
            get
            {
                return hyperPanel.ApplyOpacity;
            }
            set
            {
                hyperPanel.ApplyOpacity = value;
            }
        }

        public double MinElementScale
        {
            get
            {
                return hyperPanel.MinElementScale;
            }
            set
            {
                hyperPanel.MinElementScale = value;
            }
        }

        public HorizontalAlignment HorizontalContentAlignment
        {
            get
            {
                return hyperPanel.HorizontalContentAlignment;
            }
            set
            {
                hyperPanel.HorizontalContentAlignment = value;
            }
        }

        public VerticalAlignment VerticalContentAlignment
        {
            get
            {
                return hyperPanel.VerticalContentAlignment;
            }
            set
            {
                hyperPanel.VerticalContentAlignment = value;
            }
        }

        public double Center
        {
            get
            {
                return hyperPanel.Center;
            }
            set
            {
                hyperPanel.Center = value;
            }
        }

        #endregion
    }
}
