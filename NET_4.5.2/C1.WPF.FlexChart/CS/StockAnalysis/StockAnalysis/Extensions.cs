using C1.Chart;
using System;

#if UWP
using Windows.UI;
using Windows.UI.Xaml.Media;
namespace C1.Xaml.Chart
#else
using System.Windows.Media;
namespace StockAnalysis
#endif
{
#if WPF
    [SmartAssembly.Attributes.DoNotObfuscateType]
#endif
    static class Extensions
    {
        static public int ToArgb(this Color clr)
        {
            return (clr.A << 24) | (clr.R << 16) | (clr.G << 8) | clr.B;
        }

        static public Color FromArgb(this int clr)
        {
            var bytes = BitConverter.GetBytes(clr);
            return Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
        }
    }
}