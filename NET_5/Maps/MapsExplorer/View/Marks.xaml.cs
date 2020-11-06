using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using C1.WPF.Maps;

namespace MapsExplorer
{
    public partial class Marks : UserControl
    {
        VectorLayer vl;
        Random rnd = new Random();
        VectorPlacemark current = null;
        Point offset = new Point();
        int idx = 1;

        public Marks()
        {
            InitializeComponent();
            Tag = "Marks";
            vl = new VectorLayer();
            maps.Layers.Add(vl);
            maps.MouseLeftButtonUp += new MouseButtonEventHandler(maps_MouseLeftButtonUp);

            for (int i = 0; i < 10; i++)
            {
                // create random coordinates
                Point pt = new Point(-80 + rnd.Next(160), -80 + rnd.Next(160));
                AddMark(pt);
            }
        }

        void maps_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                AddMark(maps.ScreenToGeographic(e.GetPosition(maps)));
            }
        }

        void mark_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VectorPlacemark pm = (VectorPlacemark)sender;

            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                e.Handled = true;
                if (pm.CaptureMouse())
                {
                    current = pm;
                    offset = e.GetPosition(current);
                }
            }
            else if (Keyboard.Modifiers == ModifierKeys.Alt)
            {
                e.Handled = true;
                vl.Children.Remove(pm);
            }
        }

        void AddMark(Point pt)
        {
            Color clr = Utils.GetRandomColor(128, 192);
            VectorPlacemark mark = new VectorPlacemark()
            {
                GeoPoint = pt,
                Label = new TextBlock()
                {
                    RenderTransform = new TranslateTransform() { Y = -5 },
                    IsHitTestVisible = false,
                    Text = idx.ToString()
                },
                LabelPosition = LabelPosition.Top,
                Geometry = Utils.CreateBaloon(),
                Stroke = new SolidColorBrush(Colors.DarkGray),
                Fill = new SolidColorBrush(clr),
            };

            mark.MouseLeftButtonDown += new MouseButtonEventHandler(mark_MouseLeftButtonDown);
            mark.MouseLeftButtonUp += new MouseButtonEventHandler(mark_MouseLeftButtonUp);
            mark.MouseMove += new MouseEventHandler(mark_MouseMove);
            vl.Children.Add(mark);
            vl.LabelVisibility = LabelVisibility.Visible;
            idx++;
        }

        void mark_MouseMove(object sender, MouseEventArgs e)
        {
            if (current != null)
            {
                Point pt = e.GetPosition(maps);
                pt.X -= offset.X; pt.Y -= offset.Y;
                current.GeoPoint = maps.ScreenToGeographic(pt);
            }
        }

        void mark_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            VectorPlacemark pm = (VectorPlacemark)sender;
            if (pm != null)
                pm.ReleaseMouseCapture();
            current = null;
        }
    }
}
