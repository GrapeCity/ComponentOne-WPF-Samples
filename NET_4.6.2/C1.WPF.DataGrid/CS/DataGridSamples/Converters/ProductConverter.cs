using System;
using System.Globalization;
using System.Windows.Data;

namespace DataGridSamples
{
    public class ProductGroupConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                string text = ((string)value).Trim();
                if (text.Length > 1)
                {
                    return text.Substring(0, 2).ToUpper();
                }
                return "";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }

    public class ProductGroupNameConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string && targetType.Name == "String")
            {
                switch (value as string)
                {
                    case "BB":
                        return "BB - Bottom Bracket";
                    case "BC":
                        return "BC - Bottle Cage";
                    case "BK":
                        return "BK - Bike";
                    case "CA":
                        return "CA - Cycling Cap";
                    case "CH":
                        return "CH - Chain";
                    case "CL":
                        return "CL - Bike Wash";
                    case "CS":
                        return "CS - Crankset";
                    case "FB":
                        return "FB - Front Brakes";
                    case "FD":
                        return "FD - Front Derailleur";
                    case "FE":
                        return "FE - Fender Set";
                    case "FK":
                        return "FK - Fork";
                    case "FR":
                        return "FR - Frame";
                    case "FW":
                        return "FW - Front Wheel";
                    case "GL":
                        return "GL - Gloves";
                    case "HB":
                        return "HB - Handlebars";
                    case "HL":
                        return "HL - Helmet";
                    case "HS":
                        return "HS - Headset";
                    case "HY":
                        return "HY - Hydration";
                    case "LJ":
                        return "LJ - Logo Jersey";
                    case "LO":
                        return "LO - Cable Lock";
                    case "LT":
                        return "LT - Lights";
                    case "PA":
                        return "PA - Panniers";
                    case "PD":
                        return "PD - Pedal";
                    case "PK":
                        return "PK - Patch kit";
                    case "PU":
                        return "PU - Pump";
                    case "RA":
                        return "RA - Hitch Rack";
                    case "RB":
                        return "RB - Rear Brakes";
                    case "RD":
                        return "RD - Rear Derailleur";
                    case "RW":
                        return "RW - Rear Wheel";
                    case "SB":
                        return "SB - Bib-Shorts";
                    case "SE":
                        return "SE - Seat";
                    case "SH":
                        return "SH - Shorts";
                    case "SJ":
                        return "SJ - Classic Jersey";
                    case "SO":
                        return "SO - Socks";
                    case "ST":
                        return "ST - Stand";
                    case "TG":
                        return "TG - Tights";
                    case "TI":
                        return "TI - Tires";
                    case "TT":
                        return "TT - Tire Tube";
                    case "VE":
                        return "VE - Vest";
                    case "WB":
                        return "WB - Water Bottle ";
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}
