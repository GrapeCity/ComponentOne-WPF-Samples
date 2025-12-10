using Localization.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Localization.Service
{
    /// <summary>
    /// Provides functionality for managing and applying culture-specific localization settings.
    /// </summary>
    public class CultureLocalizationService : ICultureLocalizationService
    {
        private static readonly string[] _cultureNames = new[]
        {
            "ar", "cs", "da", "de", "el", "es", "fi", "fr", "he", "it",
            "ja", "nl", "no", "pl", "pt", "ro", "ru", "en", "sk", "sv", "tr",
            "zh", "zh-Hans", "zh-Hant"
        };

        /// <summary>
        /// Retrieves a list of supported cultures based on the configured culture names.
        /// </summary>
        public List<CultureInfo> GetSupportedCultures() =>
            _cultureNames.Select(c => new CultureInfo(c)).ToList();

        /// <summary>
        /// Retrieves the closest matching supported culture for the current thread's UI culture,  or a fallback culture
        /// if no match is found.
        /// </summary>
        public CultureInfo GetClosestMatchOrDefault()
        {
            var currentLang = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            return GetSupportedCultures().FirstOrDefault(c => c.TwoLetterISOLanguageName == currentLang)
                ?? GetFallback();
        }

        /// <summary>
        /// Retrieves the fallback culture to be used when no specific culture is available.
        /// </summary>
        public CultureInfo GetFallback() => new CultureInfo("en-US");


        /// <summary>
        /// Applies the specified culture to the current thread and updates the language settings.
        /// </summary>
        public void ApplyCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }

}
