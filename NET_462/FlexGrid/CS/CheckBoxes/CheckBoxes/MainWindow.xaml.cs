using C1.WPF.FlexGrid;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CheckBoxes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // populate bound grid
            int i = 0;
            List<Rect> list = new List<Rect>();
            for (i = 1; i <= 100; i++)
            {
                list.Add(new Rect(i, i, i, i));
            }
            _fgBound.ItemsSource = list;

            // populate unbound grid
            for (i = 1; i <= 10; i++)
            {
                _fgUnbound.Rows.Add(new Row());
                _fgUnbound.Columns.Add(new Column());
            }

            // add some simple values to the cells
            _fgUnbound[0, 0] = "Hello World";
            _fgUnbound[1, 0] = 123.456;
            _fgUnbound[2, 0] = new DateTime(2012, 10, 5);
            _fgUnbound[3, 0] = true;
            _fgUnbound[4, 0] = false;

            // and add some controls too
            CheckBox chk = new CheckBox();
            ToolTipService.SetToolTip(chk, "This CheckBox is stored in a grid cell.");
            chk.VerticalAlignment = VerticalAlignment.Center;
            chk.HorizontalAlignment = HorizontalAlignment.Center;
            _fgUnbound[0, 1] = chk;

            ComboBox cmb = new ComboBox();
            cmb.Items.Add("First");
            cmb.Items.Add("Second");
            cmb.Items.Add("Third");
            ToolTipService.SetToolTip(cmb, "This ComboBox is stored in a grid cell.");
            _fgUnbound[1, 1] = cmb;

            // connect custom cell factory to unbound grid
            // (to show booleans as checkboxes)
            _fgUnbound.CellFactory = new MyCellFactory();

            _fgUnbound.BeginningEdit += _fgUnbound_BeginningEdit;
        }

        private void _fgUnbound_BeginningEdit(object sender, CellEditEventArgs e)
        {
            var ctr = _fgUnbound.Cells[e.Row, e.Column] as Control;
            if(ctr is CheckBox || ctr is ComboBox)
            {
                e.Cancel = true;
            }
        }
    }
}
