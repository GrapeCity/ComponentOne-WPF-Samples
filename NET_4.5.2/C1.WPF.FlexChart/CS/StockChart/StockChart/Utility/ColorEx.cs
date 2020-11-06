using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace StockChart
{
    #region Structs
    // An HSB color
    public struct HsbColor
    {
        public double A;
        public double H;
        public double S;
        public double B;
    }
    #endregion
    public class ColorEx
    {
        public static HsbColor RgbToHsb(Color rgbColor)
        {
            /* Hue values range between 0 and 360. All 
             * other values range between 0 and 1. */

            // Create HSB color object
            var hsbColor = new HsbColor();

            // Get RGB color component values
            var r = (int)rgbColor.R;
            var g = (int)rgbColor.G;
            var b = (int)rgbColor.B;
            var a = (int)rgbColor.A;

            // Get min, max, and delta values
            double min = Math.Min(Math.Min(r, g), b);
            double max = Math.Max(Math.Max(r, g), b);
            double delta = max - min;

            /* Black (max = 0) is a special case. We 
             * simply set HSB values to zero and exit. */

            // Black: Set HSB and return
            if (max == 0.0)
            {
                hsbColor.H = 0.0;
                hsbColor.S = 0.0;
                hsbColor.B = 0.0;
                hsbColor.A = a / 255;
                return hsbColor;
            }

            /* Now we process the normal case. */

            // Set HSB Alpha value
            var alpha = (double)a;
            hsbColor.A = alpha / 255;

            // Set HSB Hue value
            if (r == max) hsbColor.H = (g - b) / delta;
            else if (g == max) hsbColor.H = 2 + (b - r) / delta;
            else if (b == max) hsbColor.H = 4 + (r - g) / delta;
            hsbColor.H *= 60;
            if (hsbColor.H < 0.0) hsbColor.H += 360;

            // Set other HSB values
            hsbColor.S = delta / max;
            hsbColor.B = max / 255;

            // Set return value
            return hsbColor;
        }

        public static Color HsbToRgb(HsbColor hsbColor)
        {
            /* Gray (zero saturation) is a special case.We simply
             * set RGB values to HSB Brightness value and exit. */

            // Gray: Set RGB and return
            if (hsbColor.S == 0.0)
            {
                return Color.FromArgb((byte)(hsbColor.A * 255),
                                    (byte)(hsbColor.B * 255),
                                    (byte)(hsbColor.B * 255),
                                    (byte)(hsbColor.B * 255)
                                    );
            }

            /* Now we process the normal case. */
            var h = (hsbColor.H == 360) ? 0 : hsbColor.H / 60;
            //var i = (int)(Math.Truncate(h));

            //var i = (int)(Math.Round(h));
            if (double.IsNaN(h))
            {
                return Color.FromArgb((byte)(hsbColor.A * 255),
                                    (byte)(hsbColor.B * 255),
                                    (byte)(hsbColor.B * 255),
                                    (byte)(hsbColor.B * 255)
                                    );
            }


            int i = Convert.ToInt32(Math.Floor(h));
            var f = h - i;

            var p = hsbColor.B * (1.0 - hsbColor.S);
            var q = hsbColor.B * (1.0 - (hsbColor.S * f));
            var t = hsbColor.B * (1.0 - (hsbColor.S * (1.0 - f)));

            double r, g, b;
            switch (i)
            {
                case 0:
                    r = hsbColor.B;
                    g = t;
                    b = p;
                    break;

                case 1:
                    r = q;
                    g = hsbColor.B;
                    b = p;
                    break;

                case 2:
                    r = p;
                    g = hsbColor.B;
                    b = t;
                    break;

                case 3:
                    r = p;
                    g = q;
                    b = hsbColor.B;
                    break;

                case 4:
                    r = t;
                    g = p;
                    b = hsbColor.B;
                    break;

                default:
                    r = hsbColor.B;
                    g = p;
                    b = q;
                    break;
            }


            // Set return value
            return Color.FromArgb((byte)(hsbColor.A * 255),
                (byte)(r * 255),
                (byte)(g * 255),
                (byte)(b * 255)
                );
        }
    }
}
