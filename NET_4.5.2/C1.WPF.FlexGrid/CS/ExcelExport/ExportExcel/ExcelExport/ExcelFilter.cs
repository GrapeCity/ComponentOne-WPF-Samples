using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using C1.WPF.Excel;
using C1.WPF.FlexGrid;
using C1.WPF;

using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ExcelExport
{

    /// <summary>
    /// Class that provides methods for transferring data between an XLSheet and a C1FlexGrid.
    /// </summary>
    internal sealed class ExcelFilter
    {
        private static C1XLBook _lastBook;
        private static Dictionary<XLStyle, ExcelCellStyle> _cellStyles = new Dictionary<XLStyle, ExcelCellStyle>();

        private static Dictionary<ExcelCellStyle, XLStyle> _excelStyles = new Dictionary<ExcelCellStyle, XLStyle>();
        //---------------------------------------------------------------------------------
        #region "** object model"

        /// <summary>
        /// Saves the content of a C1FlexGrid into an XLSheet.
        /// </summary>
        public static void Save(C1FlexGrid flex, XLSheet sheet)
        {
            // clear style cache if this is a new book
            if (!object.ReferenceEquals(sheet.Book, _lastBook))
            {
                _cellStyles.Clear();
                _excelStyles.Clear();
                _lastBook = sheet.Book;
            }

            // save global parameters
            sheet.DefaultRowHeight = PixelsToTwips(flex.Rows.DefaultSize);
            sheet.DefaultColumnWidth = PixelsToTwips(flex.Columns.DefaultSize);
            sheet.Locked = flex.IsReadOnly;
            sheet.ShowGridLines = flex.GridLinesVisibility != GridLinesVisibility.None;
            sheet.ShowHeaders = flex.HeadersVisibility != HeadersVisibility.None;
            sheet.OutlinesBelow = flex.GroupRowPosition == GroupRowPosition.BelowData;

            // save columns
            sheet.Columns.Clear();
            foreach (Column col in flex.Columns)
            {
                dynamic c = sheet.Columns.Add();
                if (!col.Width.IsAuto)
                {
                    c.Width = PixelsToTwips(col.ActualWidth);
                }
                c.Visible = col.Visible;
                if (col.CellStyle is ExcelCellStyle)
                {
                    c.Style = GetXLStyle(flex, sheet, (ExcelCellStyle)col.CellStyle);
                }
            }

            sheet.Rows.Clear();

            //save column headers
            XLStyle headerStyle = default(XLStyle);
            headerStyle = new XLStyle(sheet.Book);
            headerStyle.Font = new XLFont("Arial", 10, true, false);

            foreach (Row row in flex.ColumnHeaders.Rows)
            {
                dynamic r = sheet.Rows.Add();
                if (row.Height > -1)
                {
                    r.Height = PixelsToTwips(row.Height);
                }
                if (row.CellStyle is ExcelCellStyle)
                {
                    r.Style = GetXLStyle(flex, sheet, (ExcelCellStyle)row.CellStyle);
                }
                if (row is ExcelRow)
                {
                    r.OutlineLevel = ((ExcelRow)row).Level;
                }
                for (int c = 0; c <= flex.ColumnHeaders.Columns.Count - 1; c++)
                {
                    // save cell value
                    dynamic cell = sheet[row.Index, c];
                    string colHeader = flex.ColumnHeaders[row.Index, c] != null ? flex.ColumnHeaders[row.Index, c].ToString() : flex.Columns[c].ColumnName;
                    cell.Value = colHeader;

                    // make column headers bold
                    cell.Style = headerStyle;

                }
                r.Visible = row.Visible;
            }



            // save rows
            foreach (Row row in flex.Rows)
            {
                dynamic r = sheet.Rows.Add();
                if (row.Height > -1)
                {
                    r.Height = PixelsToTwips(row.Height);
                }
                if (row.CellStyle is ExcelCellStyle)
                {
                    r.Style = GetXLStyle(flex, sheet, (ExcelCellStyle)row.CellStyle);
                }
                if (row is ExcelRow)
                {
                    r.OutlineLevel = ((ExcelRow)row).Level;
                }
                r.Visible = row.Visible;
            }

            int rowStart = flex.ColumnHeaders.Rows.Count;
            // save cells
            for (int r = 0; r <= flex.Rows.Count - 1; r++, rowStart++)
            {
                for (int c = 0; c <= flex.Columns.Count - 1; c++)
                {
                    // save cell value
                    dynamic cell = sheet[rowStart, c];
                   
                    dynamic obj = flex[r, c];
                    cell.Value = obj is FrameworkElement ? 0 : obj;

                    // save cell formula and style
                    dynamic row = flex.Rows[r] as ExcelRow;
                    if (row != null)
                    {
                        // save cell formula
                        dynamic col = flex.Columns[c];

                        // save cell style
                        dynamic cs = row.GetCellStyle(col) as ExcelCellStyle;
                        if (cs != null)
                        {
                            cell.Style = GetXLStyle(flex, sheet, cs);
                        }
                    }
                }
            }

            // save selection
            dynamic sel = flex.Selection;
            if (sel.IsValid)
            {
                dynamic xlSel = new XLCellRange(sheet, sel.Row, sel.Row2, sel.Column, sel.Column2);
                sheet.SelectedCells.Clear();
                sheet.SelectedCells.Add(xlSel);
            }
        }

        #endregion

        //---------------------------------------------------------------------------------
        #region "** implementation"

        private static double TwipsToPixels(double twips)
        {
            return Convert.ToInt32(twips / 1440.0 * 96.0 * 1.2 + 0.5);
        }
        private static int PixelsToTwips(double pixels)
        {
            return Convert.ToInt32(pixels * 1440.0 / 96.0 / 1.2 + 0.5);
        }
        private static double PointsToPixels(double points)
        {
            return points / 72.0 * 96.0 * 1.2;
        }
        private static double PixelsToPoints(double pixels)
        {
            return pixels * 72.0 / 96.0 / 1.2;
        }

        // convert excel styles into grid styles
        private static ExcelCellStyle GetCellStyle(XLStyle x)
        {
            // look it up in the cache
            ExcelCellStyle s = default(ExcelCellStyle);
            if (_cellStyles.TryGetValue(x, out s))
            {
                return s;
            }

            // not found, create style now
            s = new ExcelCellStyle();

            // alignment
            switch (x.AlignHorz)
            {
                case XLAlignHorzEnum.Left:
                    s.HorizontalAlignment = HorizontalAlignment.Left;
                    break;
                case XLAlignHorzEnum.Center:
                    s.HorizontalAlignment = HorizontalAlignment.Center;
                    break;
                case XLAlignHorzEnum.Right:
                    s.HorizontalAlignment = HorizontalAlignment.Right;
                    break;
            }
            switch (x.AlignVert)
            {
                case XLAlignVertEnum.Top:
                    s.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case XLAlignVertEnum.Center:
                    s.VerticalAlignment = VerticalAlignment.Center;
                    break;
                case XLAlignVertEnum.Bottom:
                    s.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
            }
            s.TextWrapping = x.WordWrap;

            // colors
            if (x.BackPattern == XLPatternEnum.Solid && IsColorValid(x.BackColor))
            {
                s.Background = new SolidColorBrush(x.BackColor);
            }
            if (IsColorValid(x.ForeColor))
            {
                s.Foreground = new SolidColorBrush(x.ForeColor);
            }

            // font
            dynamic font = x.Font;
            if (font != null)
            {
                s.FontFamily = new FontFamily(font.FontName);
                s.FontSize = PointsToPixels(font.FontSize);
                if (font.Bold)
                {
                    s.FontWeight = FontWeights.Bold;
                }
                if (font.Italic)
                {
                    s.FontStyle = FontStyles.Italic;
                }
                if (font.Underline != XLUnderlineStyle.None)
                {
                    s.TextDecorations = TextDecorations.Underline;
                }
            }

            // format
            if (!string.IsNullOrEmpty(x.Format))
            {
                s.Format = XLStyle.FormatXLToDotNet(x.Format);
            }

            // borders
            s.CellBorderThickness = new Thickness(GetBorderThickness(x.BorderLeft), GetBorderThickness(x.BorderTop), GetBorderThickness(x.BorderRight), GetBorderThickness(x.BorderBottom));
            s.CellBorderBrushLeft = GetBorderBrush(x.BorderColorLeft);
            s.CellBorderBrushTop = GetBorderBrush(x.BorderColorTop);
            s.CellBorderBrushRight = GetBorderBrush(x.BorderColorRight);
            s.CellBorderBrushBottom = GetBorderBrush(x.BorderColorBottom);

            // save in cache and return
            _cellStyles[x] = s;
            return s;
        }

        // convert grid styles into excel styles
        private static XLStyle GetXLStyle(C1FlexGrid flex, XLSheet sheet, ExcelCellStyle s)
        {
            // look it up in the cache
            XLStyle x = default(XLStyle);
            if (_excelStyles.TryGetValue(s,out  x))
            {
                return x;
            }

            // not found, create style now
            x = new XLStyle(sheet.Book);

            // alignment
            if (s.HorizontalAlignment.HasValue)
            {
                switch (s.HorizontalAlignment.Value)
                {
                    case HorizontalAlignment.Left:
                        x.AlignHorz = XLAlignHorzEnum.Left;
                        break;
                    case HorizontalAlignment.Center:
                        x.AlignHorz = XLAlignHorzEnum.Center;
                        break;
                    case HorizontalAlignment.Right:
                        x.AlignHorz = XLAlignHorzEnum.Right;
                        break;
                }
            }
            if (s.VerticalAlignment.HasValue)
            {
                switch (s.VerticalAlignment.Value)
                {
                    case VerticalAlignment.Top:
                        x.AlignVert = XLAlignVertEnum.Top;
                        break;
                    case VerticalAlignment.Center:
                        x.AlignVert = XLAlignVertEnum.Center;
                        break;
                    case VerticalAlignment.Bottom:
                        x.AlignVert = XLAlignVertEnum.Bottom;
                        break;
                }
            }
            if (s.TextWrapping.HasValue)
            {
                x.WordWrap = s.TextWrapping.Value;
            }

            // colors
            if (s.Background is SolidColorBrush)
            {
                x.BackColor = ((SolidColorBrush)s.Background).Color;
                x.BackPattern = XLPatternEnum.Solid;
            }
            if (s.Foreground is SolidColorBrush)
            {
                x.ForeColor = ((SolidColorBrush)s.Foreground).Color;
            }

            // font
            dynamic fontName = flex.FontFamily.Source;
            dynamic fontSize = flex.FontSize;
            dynamic bold = false;
            dynamic italic = false;
            bool underline = false;
            bool hasFont = false;
            if (s.FontFamily != null)
            {
                fontName = s.FontFamily.Source;
                hasFont = true;
            }
            if (s.FontSize.HasValue)
            {
                fontSize = s.FontSize.Value;
                hasFont = true;
            }
            if (s.FontWeight.HasValue)
            {
                bold = s.FontWeight.Value == FontWeights.Bold || s.FontWeight.Value == FontWeights.ExtraBold || s.FontWeight.Value == FontWeights.SemiBold;
                hasFont = true;
            }
            if (s.FontStyle.HasValue)
            {
                italic = s.FontStyle.Value == FontStyles.Italic;
                hasFont = true;
            }
            if (s.TextDecorations != null)
            {
                underline = true;
                hasFont = true;
            }
            if (hasFont)
            {
                fontSize = PixelsToPoints(fontSize);
                if (underline)
                {
                    dynamic color = Colors.Black;
                    if (flex.Foreground is SolidColorBrush)
                    {
                        color = ((SolidColorBrush)flex.Foreground).Color;
                    }
                    if (s.Foreground is SolidColorBrush)
                    {
                        color = ((SolidColorBrush)s.Foreground).Color;
                    }
                    x.Font = new XLFont(fontName, Convert.ToSingle(fontSize), bold, italic, false, XLFontScript.None, XLUnderlineStyle.Single, color);
                }
                else
                {
                    x.Font = new XLFont(fontName, Convert.ToSingle(fontSize), bold, italic);
                }
            }

            // format
            if (!string.IsNullOrEmpty(s.Format))
            {
                x.Format = XLStyle.FormatDotNetToXL(s.Format);
            }

            // borders
            if (s.CellBorderThickness.Left > 0 && s.CellBorderBrushLeft is SolidColorBrush)
            {
                x.BorderLeft = GetBorderLineStyle(s.CellBorderThickness.Left);
                x.BorderColorLeft = ((SolidColorBrush)s.CellBorderBrushLeft).Color;
            }
            if (s.CellBorderThickness.Top > 0 && s.CellBorderBrushTop is SolidColorBrush)
            {
                x.BorderTop = GetBorderLineStyle(s.CellBorderThickness.Top);
                x.BorderColorTop = ((SolidColorBrush)s.CellBorderBrushTop).Color;
            }
            if (s.CellBorderThickness.Right > 0 && s.CellBorderBrushRight is SolidColorBrush)
            {
                x.BorderRight = GetBorderLineStyle(s.CellBorderThickness.Right);
                x.BorderColorRight = ((SolidColorBrush)s.CellBorderBrushRight).Color;
            }
            if (s.CellBorderThickness.Bottom > 0 && s.CellBorderBrushBottom is SolidColorBrush)
            {
                x.BorderBottom = GetBorderLineStyle(s.CellBorderThickness.Bottom);
                x.BorderColorBottom = ((SolidColorBrush)s.CellBorderBrushBottom).Color;
            }

            // save in cache and return
            _excelStyles[s] = x;
            return x;
        }
        private static double GetBorderThickness(XLLineStyleEnum ls)
        {
            switch (ls)
            {
                case XLLineStyleEnum.None:
                    return 0;
                case XLLineStyleEnum.Hair:
                    return 0.5;
                case XLLineStyleEnum.Thin:
                case XLLineStyleEnum.ThinDashDotDotted:
                case XLLineStyleEnum.ThinDashDotted:
                case XLLineStyleEnum.Dashed:
                case XLLineStyleEnum.Dotted:
                    return 1;
                case XLLineStyleEnum.Medium:
                case XLLineStyleEnum.MediumDashDotDotted:
                case XLLineStyleEnum.MediumDashDotted:
                case XLLineStyleEnum.MediumDashed:
                case XLLineStyleEnum.SlantedMediumDashDotted:
                    return 2;
                case XLLineStyleEnum.Double:
                case XLLineStyleEnum.Thick:
                    return 3;
            }
            return 0;
        }
        private static XLLineStyleEnum GetBorderLineStyle(double t)
        {
            if (t == 0)
            {
                return XLLineStyleEnum.None;
            }
            if (t < 1)
            {
                return XLLineStyleEnum.Hair;
            }
            if (t < 2)
            {
                return XLLineStyleEnum.Thin;
            }
            if (t < 3)
            {
                return XLLineStyleEnum.Medium;
            }
            return XLLineStyleEnum.Thick;
        }
        private static Brush GetBorderBrush(Color color)
        {
            return IsColorValid(color) ? new SolidColorBrush(color) : null;
        }
        private static bool IsColorValid(Color color)
        {
            return color.A > 0;
            // == 0xff;
        }

        #endregion
    }
}


