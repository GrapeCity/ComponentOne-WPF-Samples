using C1.WPF.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ThemeExplorer
{
    public class CustomTheme : C1Theme
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTheme"/> class.
        /// </summary>
        public CustomTheme() 
               : base(typeof(CustomTheme).Assembly, "ThemeExplorer.CustomTheme.CustomMSTheme.xaml",
                 typeof(CustomTheme).Assembly, "ThemeExplorer.CustomTheme.CustomTheme.xaml")
        {
            // ThemeExplorer\CustomTheme\CustomMSTheme.xaml file contains resources for Microsoft controls.
            // ThemeExplorer\CustomTheme\CustomTheme.xaml file contains resources for ComponentOne controls.
            // Both ResourceDictionaries have Build Action set to EmbeddedResource.
            // It allows to use the same theme files in different applications even if application doesn't reference some assemblies.

            // To customize theme font, embed font *.ttf files into your application and set DefaulFontFamily as below.
            // Multiple font families separated by comma specify font fallback order.

            //DefaultFontFamily = "./#Roboto, ./#Noto Sans CJK JP, ./#Noto Sans CJK KR, ./#Noto Sans CJK SC, ./#Noto Sans CJK TC, ./#Noto Sans CJK HK";


            // Uncomment below line to customize font size

            //FontSize = 14;
        }
    }
}
