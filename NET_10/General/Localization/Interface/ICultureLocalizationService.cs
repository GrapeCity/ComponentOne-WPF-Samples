using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Interface
{
    /// <summary>
    /// Defines a service for managing culture-specific localization operations.
    /// </summary>
    public interface ICultureLocalizationService
    {
        List<CultureInfo> GetSupportedCultures();
        CultureInfo GetClosestMatchOrDefault();
        CultureInfo GetFallback();
        void ApplyCulture(CultureInfo culture);
    }
}
