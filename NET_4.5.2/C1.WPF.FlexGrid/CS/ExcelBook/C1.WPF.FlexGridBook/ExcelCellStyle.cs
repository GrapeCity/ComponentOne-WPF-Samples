using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using C1.WPF.FlexGrid;

namespace C1.WPF.FlexGridBook
{
    /// <summary>
    /// Extends the CellStyle class to provide Excel-style cell borders and a format string.
    /// </summary>
    public class ExcelCellStyle : CellStyle
    {
        // ** fields
        string _format;
        Thickness _bdrThickness;
        Brush _bdrLeft, _bdrTop, _bdrRight, _bdrBottom;
        static Thickness _thicknessEmpty = new Thickness(0);

        // ** object model
        public string Format
        {
            get { return _format; }
            set 
            {
                if (value != _format)
                {
                    _format = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Format"));
                }
            }
        }
        public Thickness CellBorderThickness
        {
            get { return _bdrThickness; }
            set
            {
                if (value != _bdrThickness)
                {
                    _bdrThickness = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("BorderThickness"));
                }
            }
        }
        public Brush CellBorderBrushLeft
        {
            get { return _bdrLeft; }
            set
            {
                if (value != _bdrLeft)
                {
                    _bdrLeft = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("BorderColorLeft"));
                }
            }
        }
        public Brush CellBorderBrushTop
        {
            get { return _bdrTop; }
            set
            {
                if (value != _bdrTop)
                {
                    _bdrTop = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("BorderColorTop"));
                }
            }
        }
        public Brush CellBorderBrushRight
        {
            get { return _bdrRight; }
            set
            {
                if (value != _bdrRight)
                {
                    _bdrRight = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("BorderColorRight"));
                }
            }
        }
        public Brush CellBorderBrushBottom
        {
            get { return _bdrBottom; }
            set
            {
                if (value != _bdrBottom)
                {
                    _bdrBottom = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("BorderColorBottom"));
                }
            }
        }

        // ** overrides
        public override void Apply(Border bdr, SelectedState selState)
        {
            base.Apply(bdr, selState);
            ApplyBorder(bdr, _bdrLeft, new Thickness(_bdrThickness.Left, 0, 0, 0));
            ApplyBorder(bdr, _bdrTop, new Thickness(0, _bdrThickness.Top, 0, 0));
            ApplyBorder(bdr, _bdrRight, new Thickness(0, 0, _bdrThickness.Right, 0));
            ApplyBorder(bdr, _bdrBottom, new Thickness(0, 0, 0, _bdrThickness.Bottom));
        }
        void ApplyBorder(Border bdr, Brush br, Thickness t)
        {
            if (br != null && t != _thicknessEmpty)
            {
                // create inner border
                var inner = new Border();
                inner.BorderThickness = t;
                inner.BorderBrush = br;

                // transfer content
                var content = bdr.Child;
                bdr.Child = inner;
                inner.Child = content;

                // transfer padding
                inner.Padding = bdr.Padding;
                bdr.Padding = _thicknessEmpty;
            }
        }
    }
}
