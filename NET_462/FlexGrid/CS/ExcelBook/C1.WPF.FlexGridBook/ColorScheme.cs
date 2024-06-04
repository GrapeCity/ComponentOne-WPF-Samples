using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.WPF.FlexGrid;

namespace C1.WPF.FlexGridBook
{
    /// <summary>
    /// Specifies the color scheme used to display the control.
    /// </summary>
    public enum ColorScheme
    {
        /// <summary>
        /// Office 2007 blue color scheme.
        /// </summary>
        Blue,
        /// <summary>
        /// Office 2007 silver color scheme.
        /// </summary>
        Silver,
        /// <summary>
        /// Office 2007 black color scheme.
        /// </summary>
        Black
    };

    /// <summary>
    /// Class that defines and applies color schemes to C1FlexGrid controls.
    /// </summary>
    internal static class ColorSchemeManager
    {
        static Brush _brWhite = new SolidColorBrush(Colors.White);

        public static void ApplyColorScheme(C1FlexGrid grid, ColorScheme cs)
        {
            var c = PaletteProvider.GetPalette(cs);
            if (c != null)
            {
                grid.CursorBackground =
                grid.EditorBackground = null;
                grid.RowBackground =
                grid.AlternatingRowBackground =
                grid.GroupRowBackground = _brWhite;
                grid.TopLeftCellBackground = new SolidColorBrush(c[0]);
                grid.RowHeaderBackground = new SolidColorBrush(c[1]);
                grid.RowHeaderSelectedBackground = new SolidColorBrush(c[2]);
                grid.ColumnHeaderBackground = CreateGradientBrush(c[3], c[4]);
                grid.ColumnHeaderSelectedBackground = CreateGradientBrush(c[5], c[6]);
                grid.GridLinesBrush = new SolidColorBrush(c[7]); ;
                grid.HeaderGridLinesBrush = new SolidColorBrush(c[8]);
                grid.SelectionBackground = new SolidColorBrush(c[9]);
            }
        }
        static LinearGradientBrush CreateGradientBrush(Color top, Color bot)
        {
            var lgb = new LinearGradientBrush();
            var gsc = lgb.GradientStops;
            gsc.Add(new GradientStop() { Color = top, Offset = 0 });
            gsc.Add(new GradientStop() { Color = bot, Offset = 1 });
            lgb.EndPoint = new Point(0, 1);
            return lgb;
        }
        public static class PaletteProvider
        {
            public static Color[] GetPalette(ColorScheme scheme)
            {
                switch (scheme)
                {
                    case ColorScheme.Blue:
                        return new Color[]
                        {
                            Color.FromArgb(0xff, 0xa9, 0xc4, 0xe9), // brTopLeft
                            Color.FromArgb(0xff, 0xe4, 0xec, 0xf7), // brRowHdr
                            Color.FromArgb(0xff, 0xff, 0xd5, 0x8d), // brRowHdrSel 
                            Color.FromArgb(0xff, 0xf6, 0xfa, 0xfb), // brColHdr (top)
                            Color.FromArgb(0xff, 0xd5, 0xdd, 0xea), // brColHdr (bot)
                            Color.FromArgb(0xff, 0xf8, 0xd7, 0x9b), // brColHdrSel (top)
                            Color.FromArgb(0xff, 0xf1, 0xc2, 0x63), // brColHdrSel (bot)
                            Color.FromArgb(0xff, 0xd0, 0xd7, 0xe5), // brLines
                            Color.FromArgb(0xff, 0x9e, 0xb6, 0xce), // brHdrLines
                            Color.FromArgb(0xff, 0xea, 0xec, 0xf5)  // brSelection 
                        };
                    case ColorScheme.Silver:
                        return new Color[] 
                        {
                            Color.FromArgb(0xff, 0xc6, 0xc6, 0xc6), // brTopLeft
                            Color.FromArgb(0xff, 0xe7, 0xe7, 0xe7), // brRowHdr
                            Color.FromArgb(0xff, 0xf5, 0xc7, 0x95), // brRowHdrSel 
                            Color.FromArgb(0xff, 0xf1, 0xf3, 0xf3), // brColHdr (top)
                            Color.FromArgb(0xff, 0xc8, 0xc9, 0xca), // brColHdr (bot)
                            Color.FromArgb(0xff, 0xff, 0xc9, 0x96), // brColHdrSel (top)
                            Color.FromArgb(0xff, 0xff, 0x9b, 0x68), // brColHdrSel (bot)
                            Color.FromArgb(0xff, 0xd0, 0xd7, 0xe5), // brLines
                            Color.FromArgb(0xff, 0x90, 0x91, 0x92), // brHdrLines
                            Color.FromArgb(0xff, 0xea, 0xec, 0xf5)  // brSelection 
                        };
                    case ColorScheme.Black:
                        return new Color[]
                        {
                            Color.FromArgb(0xff, 0xc9, 0xc9, 0xc9), // brTopLeft
                            Color.FromArgb(0xff, 0xed, 0xed, 0xed), // brRowHdr
                            Color.FromArgb(0xff, 0xff, 0xd5, 0x8d), // brRowHdrSel 
                            Color.FromArgb(0xff, 0xf6, 0xf6, 0xf6), // brColHdr (top)
                            Color.FromArgb(0xff, 0xde, 0xde, 0xde), // brColHdr (bot)
                            Color.FromArgb(0xff, 0xf8, 0xd7, 0x9b), // brColHdrSel (top)
                            Color.FromArgb(0xff, 0xf1, 0xc1, 0x5f), // brColHdrSel (bot)
                            Color.FromArgb(0xff, 0xd0, 0xd7, 0xe5), // brLines
                            Color.FromArgb(0xff, 0xb6, 0xb6, 0xb6), // brHdrLines
                            Color.FromArgb(0xff, 0xea, 0xec, 0xf5)  // brSelection 
                        };
                }

                // invalid color scheme
                return null;
            }
        }
    }
}
