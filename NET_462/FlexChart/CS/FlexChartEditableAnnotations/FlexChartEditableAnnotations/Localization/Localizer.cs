using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;

namespace FlexChartEditableAnnotations
{
    public static class Localizer
    {
        static string currentCultureConfiguration = string.Empty;
        static XmlDocument xml = null;

        static Localizer()
        {
            switch (Thread.CurrentThread.CurrentCulture.Name)
            {
                case "en-US": currentCultureConfiguration = "FlexChartEditableAnnotations.Localization.SampleConfiguration_EN.xml"; break;
                case "ch-CH": currentCultureConfiguration = "FlexChartEditableAnnotations.Localization.SampleConfiguration_CH.xml"; break;
                case "jp-JP": currentCultureConfiguration = "FlexChartEditableAnnotations.Localization.SampleConfiguration_JP.xml"; break;
                default: currentCultureConfiguration = "FlexChartEditableAnnotations.Localization.SampleConfiguration_EN.xml"; break;
            }
        }

        public static string GetItem(string id, string attribute)
        {
            string result = String.Empty;
            try
            {
                if (xml == null)
                {
                    Stream strm = Assembly.GetExecutingAssembly().GetManifestResourceStream(currentCultureConfiguration);

                    xml = new XmlDocument();
                    xml.Load(strm);
                    strm.Close();
                }

                foreach (XmlNode n in xml.SelectNodes("/texts/text"))
                {
                    if (n.Attributes["id"].Value == id && n.SelectSingleNode(attribute) != null)
                    {
                        result = n.SelectSingleNode(attribute).InnerText;
                        break;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return string.Format("Error getting localized text: {0}", ex.Message);
            }
        }
        public static string MakeRtf(this string source)
        {

            var result = (@"{\rtf1\ansi\deff0{\colortbl \red100\green100\blue100;\red255\green0\blue0;}" + source + " }")
                .Replace("[b]", "\\b ")
                .Replace("[/b]", "\\b0 ")
                .Replace("[color]", "\\cf1 ")
                .Replace("[/color]", "\\cf0 ")
                .Replace("\r\n", "\\line ");
            return result;
        }

        public static void ClearDocument() { xml = null; }
    }
}
